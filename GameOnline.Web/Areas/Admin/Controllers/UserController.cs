using GameOnline.Core.Services.UserService.UserServiceAdmin;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IUserServiceAdmin _userService;
        #region ctor
        public UserController(IUserServiceAdmin userService)
        {
            this._userService = userService;
        }
        #endregion

        public IActionResult Index()
        {
            return View(_userService.GetUsers());
        }
    }
}
