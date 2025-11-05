using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.RoleService.Commands;

public interface IRoleServiceCommand
{
    OperationResult<int> CreateRole(CreateRoleViewmodel createRole);
    OperationResult<int> AddOrUpdateRoleForUser(AddRoleForUserViewmodel addRoleForUser);
}