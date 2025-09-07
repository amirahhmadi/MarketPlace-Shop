using GameOnline.Core.Services.ProductServices.ProductServicesAdmin;
using GameOnline.Core.Services.ProductServices.ProductServicesClient;
using GameOnline.Core.Services.SliderServices.SliderServicesClient;
using GameOnline.Core.Services.UserService.UserServiceAdmin;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISliderServiceClient _sliderServiceClient;
        private readonly IProductServicesClient _productServicesClient;

        public HomeController(ISliderServiceClient sliderServiceClient,IProductServicesClient productServicesClient)
        {
            _sliderServiceClient = sliderServiceClient;
            _productServicesClient = productServicesClient;
        }
        public IActionResult Index()
        {
            return View(_productServicesClient.GetDiscountedProducts());
        }
    }
}
