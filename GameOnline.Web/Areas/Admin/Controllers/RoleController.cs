using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Security;
using GameOnline.Core.Services.RoleService.Admin;
using GameOnline.Core.Services.RoleService.Client;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class RoleController : BaseAdminController
    {
        private readonly IRoleServiceAdmin _roleServiceAdmin;

        public RoleController(IRoleServiceAdmin roleServiceAdmin)
        {
            _roleServiceAdmin = roleServiceAdmin;
        }

        public IActionResult Index()
        {
            return View(_roleServiceAdmin.GetRoles());
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ListPermission = new PermissionListEx().permissionList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateRoleViewmodel createRole)
        {
            var result = _roleServiceAdmin.CreateRole(createRole);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Add Or Update Role For User

        [HttpGet]
        [Route("AddOrUpdateUserRole/{userId}")]
        public IActionResult AddOrUpdateUserRole(int userId)
        {
            ViewBag.ListRole = _roleServiceAdmin.GetRoles();
            return View();
        }

        [HttpPost]
        [Route("AddOrUpdateUserRole/{userId}")]
        public IActionResult AddOrUpdateUserRole(AddRoleForUserViewmodel addRoleForUser)
        {
            var result = _roleServiceAdmin.AddOrUpdateRoleForUser(addRoleForUser);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToPage("/Admin/User");
        }
        #endregion
    }
}
