using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods; // اگر چیز خاصی لازم داری نگه دار
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Users;
using Microsoft.AspNet.Identity;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin
{
    public class AccountServiceAdmin : IAccountServiceAdmin
    {
        private readonly GameOnlineContext _context;
        private readonly IUserServiceAdmin _userService;
        private readonly IHttpContextAccessor _http;
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        // تنظیمات امنیتی
        private const string CookieScheme = "GameOnline-Login";
        private static readonly TimeSpan ActivationTokenLifespan = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan ResetTokenLifespan = TimeSpan.FromMinutes(10);
        private const int MaxFailedAccessAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

        public AccountServiceAdmin(GameOnlineContext context, IUserServiceAdmin userService, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userService = userService;
            _http = contextAccessor;
        }

        // تولید توکن امن Base64Url
        private static string NewSecureToken(int bytes = 32)
        {
            var buf = new byte[bytes];
            RandomNumberGenerator.Fill(buf);
            return Convert.ToBase64String(buf)
                .Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public OperationResult<int> Register(RegisterViewmodel register)
        {
            var email = register.Email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
                return OperationResult<int>.Error("ایمیل نامعتبر است.");

            if (_userService.ExistEmail(0, email))
                return OperationResult<int>.Duplicate("این ایمیل قبلاً ثبت شده است.");

            var user = new User
            {
                Email = email,
                Password = "", // بعداً هش می‌کنیم
                Type = AccountType.NotActive,
                ActiveCode = NewSecureToken(),
                ActiveCodeExpiresAt = DateTime.UtcNow.Add(ActivationTokenLifespan),
                FailedLoginCount = 0,
                LockoutEnd = null,
                LastModified = DateTime.UtcNow,
            };

            // هش امن رمز
            user.Password = _hasher.HashPassword(user, register.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            var link = CreateUrlActiveAccount(user.Id, user.ActiveCode);
            SendEmail.Send(user.Email, "ایجاد حساب کاربری",
                $@"<p>کاربر گرامی، برای فعال‌سازی اکانت روی لینک زیر کلیک کنید:</p>
                   <p><a href=""{link}"">{link}</a></p>");

            return OperationResult<int>.Success(user.Id, "ثبت‌نام انجام شد. ایمیل فعال‌سازی ارسال شد.");
        }

        public async Task<OperationResult<int>> ActiveAccount(int userId, string activeCode)
        {
            if (string.IsNullOrWhiteSpace(activeCode))
                return OperationResult<int>.Error("کد فعال‌سازی نامعتبر است.");

            var now = DateTime.UtcNow;
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Id == userId &&
                    x.ActiveCode == activeCode &&
                    x.Type == AccountType.NotActive &&
                    x.ActiveCodeExpiresAt != null &&
                    x.ActiveCodeExpiresAt > now);

            if (user == null)
                return OperationResult<int>.NotFound("لینک فعال‌سازی نامعتبر یا منقضی است.");

            user.Type = AccountType.Active;
            user.ActiveCode = NewSecureToken(); // باطل کردن لینک قبلی
            user.ActiveCodeExpiresAt = null;
            user.LastModified = now;
            _context.SaveChanges();

            // لاگین خودکار
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name ?? " ")
            };

            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = now.AddMinutes(30)
            };

            await _http.HttpContext!.SignInAsync(
                CookieScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieScheme)),
                properties);

            return OperationResult<int>.Success(user.Id, "حساب شما فعال شد و وارد شدید.");
        }

        public async Task<OperationResult<int>> LogIn(LoginViewmodel model)
        {
            var now = DateTime.UtcNow;
            var email = model.Email?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
                return OperationResult<int>.Error("ایمیل یا رمز عبور اشتباه است.");

            // بدون Tracking برای کارایی
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return OperationResult<int>.Error("ایمیل یا رمز عبور اشتباه است.");

            // چک لاک‌اوت
            if (user.LockoutEnd != null && user.LockoutEnd > now)
                return OperationResult<int>.Error("حساب شما موقتاً قفل است. بعداً دوباره تلاش کنید.");

            if (user.Type == AccountType.NotActive)
                return OperationResult<int>.Error("حساب شما هنوز فعال نشده است.");

            if (user.Type == AccountType.Ban)
                return OperationResult<int>.Error("حساب شما مسدود است.");

            // بررسی رمز با PasswordHasher
            var verify = _hasher.VerifyHashedPassword(user, user.Password, model.Password);
            var success = verify == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success ||
                          verify == Microsoft.AspNetCore.Identity.PasswordVerificationResult.SuccessRehashNeeded;

            if (!success)
            {
                user.FailedLoginCount = (user.FailedLoginCount ?? 0) + 1;
                if (user.FailedLoginCount >= MaxFailedAccessAttempts)
                {
                    user.LockoutEnd = now.Add(LockoutDuration);
                    user.FailedLoginCount = 0; // بعد از قفل ریست می‌کنیم
                }
                user.LastModified = now;
                _context.SaveChanges();

                return OperationResult<int>.Error("ایمیل یا رمز عبور اشتباه است.");
            }

            // در صورت نیاز Rehash
            if (verify == Microsoft.AspNetCore.Identity.PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.Password = _hasher.HashPassword(user, model.Password);
            }

            // ریست شمارنده لاگین ناموفق
            user.FailedLoginCount = 0;
            user.LockoutEnd = null;
            user.LastModified = now;
            _context.SaveChanges();

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name ?? " ")
            };

            var properties = new AuthenticationProperties
            {
                IsPersistent = model.IsPersistent,
                AllowRefresh = true,
                ExpiresUtc = now.AddMinutes(30)
            };

            await _http.HttpContext!.SignInAsync(
                CookieScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieScheme)),
                properties);

            return OperationResult<int>.Success(user.Id, "ورود موفق.");
        }

        public async Task<OperationResult<int>> LogoutAsync()
        {
            await _http.HttpContext!.SignOutAsync();
            return OperationResult<int>.Success();
        }

        public OperationResult<int> FindUserByEmailForForgotPassword(string emailRaw)
        {
            var email = emailRaw?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
                return OperationResult<int>.Success(0, "اگر ایمیل معتبر باشد، لینک بازیابی ارسال می‌شود.");

            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Type == AccountType.Active);
            // پاسخ مبهم برای جلوگیری از افشای وجود حساب
            if (user == null)
                return OperationResult<int>.Success(0, "اگر ایمیل معتبر باشد، لینک بازیابی ارسال می‌شود.");

            user.ResetCode = NewSecureToken();
            user.ResetCodeExpiresAt = DateTime.UtcNow.Add(ResetTokenLifespan);
            user.LastModified = DateTime.UtcNow;
            _context.SaveChanges();

            var link = CreateUrlForgotPassword(user.Id, user.ResetCode!);
            SendEmail.Send(user.Email, "بازیابی رمز عبور",
                $@"<p>برای بازنشانی رمز عبور روی لینک زیر کلیک کنید (اعتبار {ResetTokenLifespan.TotalMinutes:0} دقیقه):</p>
                   <p><a href=""{link}"">{link}</a></p>");

            return OperationResult<int>.Success(0, "اگر ایمیل معتبر باشد، لینک بازیابی ارسال می‌شود.");
        }

        public OperationResult<int> RecoveryPassword(ForgotPasswordViewmodel model)
        {
            if (model == null || model.UserId <= 0 || string.IsNullOrWhiteSpace(model.ActiveCode))
                return OperationResult<int>.Error("درخواست نامعتبر است.");

            var now = DateTime.UtcNow;
            var user = _context.Users.FirstOrDefault(x =>
                x.Id == model.UserId &&
                x.ResetCode == model.ActiveCode &&
                x.ResetCodeExpiresAt != null &&
                x.ResetCodeExpiresAt > now);

            if (user == null)
                return OperationResult<int>.NotFound("لینک بازیابی نامعتبر یا منقضی است.");

            user.Password = _hasher.HashPassword(user, model.Password);
            user.ResetCode = NewSecureToken();          // باطل کردن لینک
            user.ResetCodeExpiresAt = null;
            user.LastModified = now;

            _context.SaveChanges();
            return OperationResult<int>.Success(user.Id, "رمز عبور با موفقیت تغییر کرد.");
        }

        public string CreateUrlActiveAccount(int userId, string activeCode)
        {
            var req = _http.HttpContext!.Request;
            var scheme = req.Scheme;
            var host = req.Host.Value;
            return $"{scheme}://{host}/ActiveAccount/{userId}/{activeCode}";
        }

        public string CreateUrlForgotPassword(int userId, string activeCode)
        {
            var req = _http.HttpContext!.Request;
            var scheme = req.Scheme;
            var host = req.Host.Value;
            return $"{scheme}://{host}/ForgotPassword/{userId}/{activeCode}";
        }
    }
}
