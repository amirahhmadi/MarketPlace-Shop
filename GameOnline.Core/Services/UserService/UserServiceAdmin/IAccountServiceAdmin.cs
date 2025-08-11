using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin;

public interface IAccountServiceAdmin
{
    OperationResult<int> Register(RegisterViewmodel register);
}