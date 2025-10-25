using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Role;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.RoleService.Admin
{
    public class RoleServiceAdmin : IRoleServiceAdmin
    {

        #region ctor

        private readonly GameOnlineContext _context;

        public RoleServiceAdmin(GameOnlineContext context)
        {
            _context = context;
        }

        #endregion


        #region GetRoles
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
        #endregion

        #region AddOrUpdateRoleForUser
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
        #endregion

        #region Create

        public OperationResult<int> CreateRole(CreateRoleViewmodel createRole)
        {

            bool existRole = ExistRole(0, createRole.RoleTitle);

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
        #endregion

        #region Exist Role
        public bool ExistRole(int RoleId, string RoleTitle)
        {
            return _context.Roles.Any(x =>
            x.RoleTitle == RoleTitle.ToLower().Trim()
            && x.Id != RoleId);
        }
        #endregion
    }
}
