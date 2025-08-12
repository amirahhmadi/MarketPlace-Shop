using System.Security.Cryptography.X509Certificates;
using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Security;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin;

public class AccountServiceAdmin : IAccountServiceAdmin
{
    private readonly GameOnlineContext _context;
    private readonly IUserServiceAdmin _userService;
    private readonly IHttpContextAccessor _contextAccessor;

    public AccountServiceAdmin(GameOnlineContext context, IUserServiceAdmin userService, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _userService = userService;
        _contextAccessor = contextAccessor;
    }

    public OperationResult<int> Register(RegisterViewmodel register)
    {
        if (_userService.ExistEmail(0, register.Email))
        {
            return OperationResult<int>.Duplicate();
        }

        User user = new User()
        {
            Email = register.Email,
            Password = register.Password.EncodePasswordMd5(),
            type = AccountType.Active,
            ActiveCode = Guid.NewGuid().ToString().Replace("-","")
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        string createLinkActiveAccount = CreateUrlActiveAccount(user.Id, user.ActiveCode);
        SendEmail.Send(user.Email, "ایجاد حساب کاربری", $"کاربر گرامی برای فعال سازی اکانت خود روی گذینه زیر کلیک کنید \r\n {createLinkActiveAccount}");

        return OperationResult<int>.Success(user.Id);
    }

    public string CreateUrlActiveAccount(int userId, string activeCode)
    {
        string scheme = _contextAccessor.HttpContext.Request.Scheme;//https or http 
        string urlSite = _contextAccessor.HttpContext.Request.Host.Value;

        return $"{scheme}://{urlSite}/ActiveAccount/{userId}/{activeCode}";
    }

    public OperationResult<int> ActiveAccount(int userId, string activeCode)
    {
        var findUser = _context.Users
            .Where(x => x.Id == userId)
            .Where(x => x.ActiveCode == activeCode)
            .Where(x => x.type == AccountType.NotActive)
            .AsNoTracking()
            .FirstOrDefault();

        if (findUser == null)
            return OperationResult<int>.NotFound();

        findUser.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
        findUser.type = AccountType.Active;

        _context.Users.Update(findUser);
        _context.SaveChanges();

        return OperationResult<int>.Success(findUser.Id);
    }
}