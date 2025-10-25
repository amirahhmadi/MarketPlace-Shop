using GameOnline.Core.ViewModels.UserViewmodel.Client;
using GameOnline.DataBase.Entities.Users;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin;

public interface IUserServiceAdmin
{
    public bool ExistEmail(int userId, string email);
    User? FindUserByEmail(string email);
    User FindUserById(int id);
    List<GetUsersViewmodel> GetUsers();
}