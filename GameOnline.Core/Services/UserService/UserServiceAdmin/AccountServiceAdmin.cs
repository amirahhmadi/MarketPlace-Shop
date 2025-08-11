using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Security;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Users;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin;

public class AccountServiceAdmin : IAccountServiceAdmin
{
    private readonly GameOnlineContext _context;
    private readonly IUserServiceAdmin _userService;
    public AccountServiceAdmin(GameOnlineContext context, IUserServiceAdmin userService)
    {
        _context = context;
        _userService = userService;
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
        SendEmail.Send(user.Email, "ایجاد حساب کاربری", "_RegisterPartial.cshtml");
        return OperationResult<int>.Success(user.Id);
    }
}