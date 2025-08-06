using GameOnline.Core.Services.SliderServices.SliderServicesClient;
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
