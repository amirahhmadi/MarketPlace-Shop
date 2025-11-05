using GameOnline.Common.Core;
using GameOnline.Core.Services.RoleService.Queries;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Role;

namespace GameOnline.Core.Services.RoleService.Commands;

public class RoleServiceCommand : IRoleServiceCommand
{
    private readonly GameOnlineContext _context;
    private readonly IRoleServiceQuery _serviceQuery;
    public RoleServiceCommand(GameOnlineContext context, IRoleServiceQuery serviceQuery)
    {
        _context = context;
        _serviceQuery = serviceQuery;
    }

    public OperationResult<int> AddOrUpdateRoleForUser(AddRoleForUserViewmodel addRoleForUser)
    {

        List<UserRole> userRole = new List<UserRole>();

        foreach (var item in addRoleForUser.RoleId)
        {
            userRole.Add(new UserRole
            {
                RoleId = item,
                CreationDate = DateTime.Now,
                UserId = addRoleForUser.UserId,
            });
        }

        _context.UserRoles.AddRange(userRole);
        _context.SaveChanges();

        return OperationResult<int>.Success(addRoleForUser.UserId);
    }

    public OperationResult<int> CreateRole(CreateRoleViewmodel createRole)
    {

        bool existRole = _serviceQuery.ExistRole(0, createRole.RoleTitle);

        if (existRole)
        {
            return OperationResult<int>.Duplicate();
        }

        Role role = new Role()
        {
            CreationDate = DateTime.Now,
            RoleTitle = createRole.RoleTitle,
        };

        _context.Roles.Add(role);
        _context.SaveChanges();

        if (role.Id > 0)
        {
            List<RolePermission> rolePermission = new List<RolePermission>();

            foreach (var item in createRole.ListPermission)
            {
                rolePermission.Add(new RolePermission
                {
                    CreationDate = DateTime.Now,
                    PermissionId = item,
                    RoleId = role.Id,
                });
            }

            _context.RolePermissions.AddRange(rolePermission);
            _context.SaveChanges();
        }
        return OperationResult<int>.Success(role.Id);
    }
}