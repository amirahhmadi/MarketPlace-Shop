using GameOnline.Common.Core;

namespace GameOnline.Core.Services.RoleService.Client
{
    public interface IRoleServiceClient
    {
        OperationResult<bool> CheckPermission(int permissionId, int userId);

    }
}
