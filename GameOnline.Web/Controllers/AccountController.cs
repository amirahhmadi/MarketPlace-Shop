using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using GameOnline.Core.Services.UserService.UserServiceAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GameOnline.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : BaseController
    {
        private const string CookieScheme = "GameOnline-Login";
        private readonly IAccountServiceAdmin _accountService;

        public AccountController(IAccountServiceAdmin accountService)
        {
            _accountService = accountService;
        }

        [HttpGet, AllowAnonymous, Route("Register")]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/");
            return View();
        }

        [HttpPost, AllowAnonymous, Route("Register")]
        public IActionResult Register(RegisterViewmodel model)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(model);
            }

            var result = _accountService.Register(model);

            if (result.IsSuccess)
                SetSweetAlert("success", "ثبت‌نام موفق!", "ایمیل تایید برای شما ارسال شد.");
            else
                SetSweetAlert("error", "خطا!", result.Message ?? "ثبت‌نام با خطا مواجه شد.");

            return View();
        }

        [HttpGet, AllowAnonymous, Route("ActiveAccount/{userId:int}/{activeCode}")]
        public IActionResult ActiveAccount(int userId, string activeCode)
        {
            var result = _accountService.ActiveAccount(userId, activeCode);
            SetSweetAlert(result.IsSuccess ? "success" : "error",
                          result.IsSuccess ? "موفق!" : "خطا!",
                          result.Message ?? "");
            return View();
        }

        [HttpGet, AllowAnonymous, Route("Login")]
        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, Route("Login")]
        public async Task<IActionResult> Login(LoginViewmodel model, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(model);
            }

            var result = await _accountService.LogIn(model);
            SetSweetAlert(result.IsSuccess ? "success" : "error",
                          result.IsSuccess ? "ورود موفق" : "خطا در ورود",
                          result.Message ?? "");

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("/");
        }

        [HttpPost, Authorize, Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.LogoutAsync();
            SetSweetAlert(result.IsSuccess ? "success" : "error",
                          result.IsSuccess ? "خروج موفق" : "خطا در خروج",
                          result.Message ?? "");
            return Redirect("/");
        }

        [HttpGet, AllowAnonymous, Route("Recovery")]
        public IActionResult Recovery()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/");
            return View();
        }

        [HttpPost, AllowAnonymous, Route("Recovery")]
        public IActionResult Recovery(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                SetSweetAlert("error", "خطا", "ایمیل الزامی است.");
                return View();
            }

            var result = _accountService.FindUserByEmailForForgotPassword(email);
            SetSweetAlert(result.IsSuccess ? "success" : "error",
                          result.IsSuccess ? "ایمیل ارسال شد" : "خطا",
                          result.Message ?? "");
            return RedirectToAction(nameof(Login));
        }

        [HttpGet, AllowAnonymous, Route("ForgotPassword/{userId:int}/{activeCode}")]
        public IActionResult ForgotPassword(int userId, string activeCode)
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/");

            var vm = new ForgotPasswordViewmodel
            {
                UserId = userId,
                ActiveCode = activeCode
            };
            return View(vm);
        }

        [HttpPost, AllowAnonymous, Route("ForgotPassword/{userId:int}/{activeCode}")]
        public IActionResult ForgotPassword(ForgotPasswordViewmodel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _accountService.RecoveryPassword(model);
            SetSweetAlert(result.IsSuccess ? "success" : "error",
                          result.IsSuccess ? "رمز عبور تغییر کرد" : "خطا",
                          result.Message ?? "");
            return RedirectToAction(nameof(Login));
        }
    }
}
