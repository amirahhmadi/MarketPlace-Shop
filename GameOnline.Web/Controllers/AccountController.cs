using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.UserService.UserServiceAdmin;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
            return View();
        }
    }
}
