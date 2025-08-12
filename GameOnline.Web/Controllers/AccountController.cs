using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.UserService.UserServiceAdmin;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;

namespace GameOnline.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountServiceAdmin _accountService;

        public AccountController(IAccountServiceAdmin accountService)
        {
            _accountService = accountService;
        }


        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewmodel register)
        {
            var result = _accountService.Register(register);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return View();
        }

        [HttpGet]
        [Route("ActiveAccount/{userId}/{activeCode}")]
        public IActionResult ActiveAccount(int userId, string activeCode)
        {
            var result = _accountService.ActiveAccount(userId, activeCode);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewmodel login)
        {
            var result = await _accountService.LogIn(login);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return Redirect("/");
        }
    }
}
