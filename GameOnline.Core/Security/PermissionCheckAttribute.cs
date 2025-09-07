using GameOnline.Core.Services.RoleService.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace GameOnline.Core.Security
{
    public class PermissionCheckAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _permissionId;

        public PermissionCheckAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roleServiceClient = (IRoleServiceClient)
                context.HttpContext.RequestServices.GetService(typeof(IRoleServiceClient));

            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = new RedirectResult("/LogIn");
                return;
            }

            var userIdClaim = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                context.Result = new RedirectResult("/LogIn");
                return;
            }

            if (roleServiceClient?.CheckPermission(_permissionId, userId).Data == false)
            {
                context.Result = new RedirectResult("/");
            }
        }
    }
}