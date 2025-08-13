using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin;

public interface IAccountServiceAdmin
{
    OperationResult<int> Register(RegisterViewmodel register);
    public OperationResult<int> ActiveAccount(int userId, string activeCode);
    public Task<OperationResult<int>> LogIn(LoginViewmodel logIn);
    public OperationResult<int> FindUserByEmailForForgotPassword(string email);
    public Task<OperationResult<int>> LogoutAsync();
    public OperationResult<int> RecoveryPassword(ForgotPasswordViewmodel forgotPassword);
}