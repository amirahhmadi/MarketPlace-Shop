using GameOnline.Core.Services.ProductServices.Queries;
using GameOnline.Core.Services.SliderServices.Queries;
using GameOnline.Core.ViewModels.UserViewmodel.Server.Account;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductServicesQuery _productServicesQuery;

        public HomeController(IProductServicesQuery productServicesQuery)
        {
            _productServicesQuery = productServicesQuery;
        }
        public IActionResult Index()
        {
            return View(_productServicesQuery.GetDiscountedProducts());
        }
    }
}
