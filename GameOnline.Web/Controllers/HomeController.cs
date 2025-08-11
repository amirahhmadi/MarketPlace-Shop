using GameOnline.Core.Services.SliderServices.SliderServicesClient;
using GameOnline.Core.Services.UserService.UserServiceAdmin;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISliderServiceClient _sliderServiceClient;
        private readonly IAccountServiceAdmin _accountServiceAdmin;

        public HomeController(ISliderServiceClient sliderServiceClient, IAccountServiceAdmin accountServiceAdmin)
        {
            _sliderServiceClient = sliderServiceClient;
            _accountServiceAdmin = accountServiceAdmin;
        }
        public IActionResult Index()
        {
            RegisterViewmodel registerViewmodel = new RegisterViewmodel()
            {
                Email = "amirrezaahmadi869@gmail.com",
                Password = "123"
            };
            _accountServiceAdmin.Register(registerViewmodel);
            return View();
        }
    }
}
