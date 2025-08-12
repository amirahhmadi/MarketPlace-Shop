using GameOnline.Core.Services.SliderServices.SliderServicesClient;
using GameOnline.Core.Services.UserService.UserServiceAdmin;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISliderServiceClient _sliderServiceClient;

        public HomeController(ISliderServiceClient sliderServiceClient)
        {
            _sliderServiceClient = sliderServiceClient;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
