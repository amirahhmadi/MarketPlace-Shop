using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;

namespace GameOnline.Core.Services.RoleService.Queries;

public interface IRoleServiceQuery
{
    List<GetRolesViewmodel> GetRoles();
    bool ExistRole(int roleId, string roleTitle);
    OperationResult<bool> CheckPermission(int permissionId, int userId);
}