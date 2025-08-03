using GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountServicesAdmin _discountServicesAdmin;
        public DiscountController(IDiscountServicesAdmin discountServicesAdmin)
        {
            _discountServicesAdmin = discountServicesAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
