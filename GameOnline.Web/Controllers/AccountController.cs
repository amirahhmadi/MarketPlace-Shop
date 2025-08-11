using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewmodel register)
        {
            return View();
        }
    }
}
