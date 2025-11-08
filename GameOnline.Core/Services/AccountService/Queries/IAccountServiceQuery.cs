using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.UserService.Commands;
using GameOnline.Core.Services.UserService.Queries;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using GameOnline.Core.Services.AccountService.Commands;

namespace GameOnline.Core.Services.AccountService.Queries;

public interface IAccountServiceQuery
{
    public Task<OperationResult<int>> LogIn(LoginViewmodel logIn);
    public OperationResult<int> FindUserByEmailForForgotPassword(string email);
    public Task<OperationResult<int>> LogoutAsync();

}

public class AccountServiceQuery : IAccountServiceQuery
{
    private readonly GameOnlineContext _context;
    private readonly IAccountServiceCommand _accountServiceCommand;
    private readonly IHttpContextAccessor _http;
    private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

    // تنظیمات امنیتی
    private const string CookieScheme = "GameOnline-Login";
    private static readonly TimeSpan ActivationTokenLifespan = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan ResetTokenLifespan = TimeSpan.FromMinutes(10);
    private const int MaxFailedAccessAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    public AccountServiceQuery(GameOnlineContext context, IAccountServiceCommand accountServiceCommand, IHttpContextAccessor http)
    {
        _context = context;
        _http = http;
        _accountServiceCommand = accountServiceCommand;
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
                new Claim(ClaimTypes.Name, user.Name ?? " "),
                new Claim(ClaimTypes.MobilePhone ,user.Phone ?? "")

            };

        var properties = new AuthenticationProperties
        {
            IsPersistent = model.IsPersistent,
            AllowRefresh = true,
            ExpiresUtc = now.AddDays(30)
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
        return OperationResult<int>.Success(1);
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

        user.ResetCode = _accountServiceCommand.NewSecureToken();
        user.ResetCodeExpiresAt = DateTime.UtcNow.Add(ResetTokenLifespan);
        user.LastModified = DateTime.UtcNow;
        _context.SaveChanges();

        var link = _accountServiceCommand.CreateUrlForgotPassword(user.Id, user.ResetCode!);
        SendEmail.Send(user.Email, "بازیابی رمز عبور",
            $@"<p>برای بازنشانی رمز عبور روی لینک زیر کلیک کنید (اعتبار {ResetTokenLifespan.TotalMinutes:0} دقیقه):</p>
                   <p><a href=""{link}"">{link}</a></p>");

        return OperationResult<int>.Success(0, "اگر ایمیل معتبر باشد، لینک بازیابی ارسال می‌شود.");
    }
}