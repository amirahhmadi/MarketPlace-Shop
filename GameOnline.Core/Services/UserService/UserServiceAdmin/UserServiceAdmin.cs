using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Users;
using Microsoft.EntityFrameworkCore;

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

    public User? FindUserByEmail(string email)
    {
        return _context.Users
            .Where(x => x.Email == email)
            .AsNoTracking()
            .FirstOrDefault();
    }

    public User FindUserById(int id)
    {
        return _context.Users
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefault();
    }
}