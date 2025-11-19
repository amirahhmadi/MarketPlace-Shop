using System.Security.Claims;
using System.Security.Cryptography;
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

namespace GameOnline.Core.Services.AccountService.Commands;

public class AccountServiceCommand : IAccountServiceCommand
{
    private readonly GameOnlineContext _context;
    private readonly IUserServiceQuery _userServiceQuery;
    private readonly IUserServiceCommand _userServiceCommand;
    private readonly IHttpContextAccessor _http;
    private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

    // تنظیمات امنیتی
    private const string CookieScheme = "GameOnline-Login";
    private static readonly TimeSpan ActivationTokenLifespan = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan ResetTokenLifespan = TimeSpan.FromMinutes(10);
    private const int MaxFailedAccessAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    public AccountServiceCommand(GameOnlineContext context, IUserServiceQuery userServiceQuery, IUserServiceCommand userServiceCommand, IHttpContextAccessor http)
    {
        _context = context;
        _userServiceQuery = userServiceQuery;
        _userServiceCommand = userServiceCommand;
        _http = http;
    }
    public OperationResult<int> Register(RegisterViewmodel register)
    {
        var email = register.Email?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email))
            return OperationResult<int>.Error("ایمیل نامعتبر است.");

        if (_userServiceQuery.ExistEmail(0, email))
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
            CreationDate = DateTime.Now
        };

        // هش امن رمز
        user.Password = _hasher.HashPassword(user, register.Password);

        _context.Users.Add(user);
        _context.SaveChanges();

        var link = CreateUrlActiveAccount(user.Id, user.ActiveCode);
        var emailBody = SendEmail.EmailTemplateHelper.GetTemplate("ActivateAccount.html", new Dictionary<string, string>
        {
            { "Name", user.Name ?? "کاربر عزیز" },
            { "Link", link },
            { "Year", DateTime.Now.Year.ToString() }
        });
        SendEmail.Send(user.Email, "فعالسازی حساب کاربری",emailBody);
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
            new Claim(ClaimTypes.Name, user.Name ?? " "),
            new Claim(ClaimTypes.MobilePhone ,user.Phone ?? "")
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

    public string NewSecureToken(int bytes = 32)
    {
        var buf = new byte[bytes];
        RandomNumberGenerator.Fill(buf);
        return Convert.ToBase64String(buf)
            .Replace("+", "-").Replace("/", "_").TrimEnd('=');
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