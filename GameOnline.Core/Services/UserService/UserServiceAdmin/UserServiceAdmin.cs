using GameOnline.DataBase.Context;

namespace GameOnline.Core.Services.UserService.UserServiceAdmin;

public class UserServiceAdmin : IUserServiceAdmin
{
    private readonly GameOnlineContext _context;
    public UserServiceAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public bool ExistEmail(int userId,string email)
    {
        return _context.Users
            .Any(x => x.Email == email && x.Id != userId);
    }

}