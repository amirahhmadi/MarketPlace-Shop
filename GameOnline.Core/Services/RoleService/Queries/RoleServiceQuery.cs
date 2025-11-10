using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.UserService.Commands;
using GameOnline.Core.Services.UserService.Queries;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.RoleService.Queries;

public class RoleServiceQuery : IRoleServiceQuery
{
    private readonly GameOnlineContext _context;
    private readonly IUserServiceQuery _userServiceQuery;

    public RoleServiceQuery(GameOnlineContext context, IUserServiceQuery userServiceQuery)
    {
        _context = context;
        _userServiceQuery = userServiceQuery;
    }

    public List<GetRolesViewmodel> GetRoles()
    {
        return _context.Roles
            .Select(x => new GetRolesViewmodel
            {
                RoleId = x.Id,
                RoleTitle = x.RoleTitle,
            })
            .AsNoTracking()
            .ToList();
    }

    public bool ExistRole(int roleId, string roleTitle)
    {
        return _context.Roles.Any(x =>
            x.RoleTitle == roleTitle.ToLower().Trim()
            && x.Id != roleId);
    }

    public OperationResult<bool> CheckPermission(int permissionId, int userId)
    {
        var findUser = _userServiceQuery.FindUserById(userId);
        if (findUser == null || findUser.IsRemove || findUser.Type == AccountType.NotActive || findUser.Type == AccountType.Ban)
        {
            return new OperationResult<bool>
            {
                IsSuccess = false,
                Code = OperationCode.Error,
                Data = false,
                Message = OperationResultMessage.NotFoundUser,
            };
        }

        var findRoleByPermission = FindRoleByPermissonId(permissionId);
        var findUserRole = FindUserRoles(userId);

        if (!findUserRole.Data.Any())
            return new OperationResult<bool>
            {
                IsSuccess = false,
                Code = OperationCode.Error,
                Data = false,
                Message = OperationResultMessage.AccessDenied,
            };

        var userRolesSet = new HashSet<int>(findUserRole.Data);
        bool checkRole = findRoleByPermission.Data.Any(userRolesSet.Contains);

        return new OperationResult<bool>
        {
            IsSuccess = checkRole,
            Code = checkRole ? OperationCode.Success : OperationCode.Error,
            Data = checkRole,
            Message = checkRole ? OperationResultMessage.Access : OperationResultMessage.AccessDenied,
        };
    }

    public OperationResult<List<int>> FindRoleByPermissonId(int permissionId)
    {
        var findRoles = _context.RolePermissions
            .Where(x => x.PermissionId.Equals(permissionId))
            .AsNoTracking()
            .Select(x => x.RoleId)
            .ToList();

        return new OperationResult<List<int>>()
        {
            Code = OperationCode.Success,
            Data = findRoles,
            IsSuccess = true,
            Message = ""
        };
    }

    public OperationResult<List<int>> FindUserRoles(int userId)
    {
        var userRole = _context.UserRoles
            .Where(x => x.UserId.Equals(userId))
            .AsNoTracking()
            .Select(x => x.RoleId)
            .ToList();

        return new OperationResult<List<int>>()
        {
            Code = OperationCode.Success,
            Data = userRole,
            IsSuccess = true,
            Message = ""
        };

    }
}