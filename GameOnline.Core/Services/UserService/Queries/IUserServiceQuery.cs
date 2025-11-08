using GameOnline.Core.ViewModels.UserViewmodel.Client;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.UserService.Queries;

public interface IUserServiceQuery
{
    public bool ExistEmail(int userId, string email);
    User? FindUserByEmail(string email);
    User FindUserById(int id);
    List<GetUsersViewmodel> GetUsers();
}

public class UserServiceQuery : IUserServiceQuery
{
    private readonly GameOnlineContext _context;
    public UserServiceQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public bool ExistEmail(int userId, string email)
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

    public List<GetUsersViewmodel> GetUsers()
    {
        return _context.Users
            .Select(x => new GetUsersViewmodel
            {
                Email = x.Email,
                UserId = x.Id
            })
            .AsNoTracking()
            .ToList();
    }
}