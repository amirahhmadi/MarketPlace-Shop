using Azure;
using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.UserService.UserServiceAdmin;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.RoleService.Client
{
    public class RoleServiceClient : IRoleServiceClient
    {
        #region ctor
        private readonly GameOnlineContext _context;
        private readonly IUserServiceAdmin _userServiceAdmin;

        public RoleServiceClient(GameOnlineContext context, IUserServiceAdmin userServiceAdmin)
        {
            _context = context;
            _userServiceAdmin = userServiceAdmin;
        }

        #endregion

        public OperationResult<bool> CheckPermission(int permissionId, int userId)
        {
            var findUser = _userServiceAdmin.FindUserById(userId);
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


        #region Get Role By PermissionId
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
        #endregion

        #region Get User Role 
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
        #endregion

    }
}
