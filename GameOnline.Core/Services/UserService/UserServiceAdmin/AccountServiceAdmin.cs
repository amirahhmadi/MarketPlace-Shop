using System.Security.Claims;
using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Security;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin
{
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

            var user = new User()
            {
                Email = register.Email,
                Password = register.Password.EncodePasswordMd5(),
                type = AccountType.NotActive, // احتمالا تو ثبت نام باید NotActive باشه تا بعدا فعال بشه
                ActiveCode = Guid.NewGuid().ToString().Replace("-", "")
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            string createLinkActiveAccount = CreateUrlActiveAccount(user.Id, user.ActiveCode);
            SendEmail.Send(user.Email, "ایجاد حساب کاربری", $"کاربر گرامی برای فعال سازی اکانت خود روی گزینه زیر کلیک کنید:\r\n{createLinkActiveAccount}");

            return OperationResult<int>.Success(user.Id);
        }

        public OperationResult<int> ActiveAccount(int userId, string activeCode)
        {
            var findUser = _context.Users
                .Where(x => x.Id == userId)
                .Where(x => x.ActiveCode == activeCode)
                .FirstOrDefault(x => x.type == AccountType.NotActive);

            if (findUser == null)
                return OperationResult<int>.NotFound();

            findUser.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
            findUser.type = AccountType.Active;

            // EF خودش تغییرات رو track می‌کنه، پس Update لازم نیست
            _context.SaveChanges();

            return OperationResult<int>.Success(findUser.Id);
        }

        public async Task<OperationResult<int>> LogIn(LoginViewmodel logIn)
        {
            var findUserByEmail = _userService.FindUserByEmail(logIn.Email);

            if (findUserByEmail == null)
                return OperationResult<int>.Error();

            if (findUserByEmail.Password != logIn.Password.EncodePasswordMd5())
                return OperationResult<int>.Error();

            if (findUserByEmail.type == AccountType.NotActive)
                return OperationResult<int>.Error();

            if (findUserByEmail.type == AccountType.Ban)
                return OperationResult<int>.Error();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, findUserByEmail.Id.ToString()),
                new Claim(ClaimTypes.Email, findUserByEmail.Email.ToLower()),
                new Claim(ClaimTypes.Name, findUserByEmail.Name ?? " ")
            };

            var properties = new AuthenticationProperties
            {
                IsPersistent = logIn.IsPersistent
            };

            await _contextAccessor.HttpContext.SignInAsync(
                "GameOnline-Login",
                new ClaimsPrincipal(new ClaimsIdentity(claims, "GameOnline-Login")),
                properties
            );

            return OperationResult<int>.Success(findUserByEmail.Id);
        }

        public string CreateUrlActiveAccount(int userId, string activeCode)
        {
            var scheme = _contextAccessor.HttpContext.Request.Scheme; // http یا https
            var urlSite = _contextAccessor.HttpContext.Request.Host.Value;

            return $"{scheme}://{urlSite}/ActiveAccount/{userId}/{activeCode}";
        }
    }
}
