using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Security;
using GameOnline.Core.Services.RoleService.Commands;
using GameOnline.Core.Services.RoleService.Queries;
using GameOnline.Core.ViewModels.RoleViewmodel.Admin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class RoleController : BaseAdminController
    {
        private IRoleServiceCommand _roleCommand;
        private IRoleServiceQuery _roleQuery;

        public RoleController(IRoleServiceCommand roleCommand, IRoleServiceQuery roleQuery)
        {
            _roleCommand = roleCommand;
            _roleQuery = roleQuery;
        }
        public IActionResult Index()
        {
            return View(_roleQuery.GetRoles());
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
            var result = _roleCommand.CreateRole(createRole);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Add Or Update Role For User

        [HttpGet]
        [Route("AddOrUpdateUserRole/{userId}")]
        public IActionResult AddOrUpdateUserRole(int userId)
        {
            ViewBag.ListRole = _roleQuery.GetRoles();
            return View();
        }

        [HttpPost]
        [Route("AddOrUpdateUserRole/{userId}")]
        public IActionResult AddOrUpdateUserRole(AddRoleForUserViewmodel addRoleForUser)
        {
            var result = _roleCommand.AddOrUpdateRoleForUser(addRoleForUser);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToPage("/Admin/User");
        }
        #endregion
    }
}
