using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using Microsoft.AspNet.Identity;

namespace GameOnline.Core.Services.AccountService.Commands;

public interface IAccountServiceCommand
{
    OperationResult<int> Register(RegisterViewmodel register);
    public Task<OperationResult<int>> ActiveAccount(int userId, string activeCode);
    public OperationResult<int> RecoveryPassword(ForgotPasswordViewmodel forgotPassword);
    string CreateUrlForgotPassword(int userId, string activeCode);
    string NewSecureToken(int bytes = 32);
}