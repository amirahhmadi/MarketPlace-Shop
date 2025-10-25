using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;

namespace GameOnline.Core.Services.RoleService.Admin
{
    public interface IRoleServiceAdmin
    {
        List<GetRolesViewmodel> GetRoles();
        OperationResult<int> CreateRole(CreateRoleViewmodel createRole);
        OperationResult<int> AddOrUpdateRoleForUser(AddRoleForUserViewmodel addRoleForUser);
    }
}
