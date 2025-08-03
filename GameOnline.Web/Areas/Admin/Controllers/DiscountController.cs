using GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class DiscountController : BaseAdminController
    {
        private readonly IDiscountServicesAdmin _discountServicesAdmin;
        public DiscountController(IDiscountServicesAdmin discountServicesAdmin)
        {
            _discountServicesAdmin = discountServicesAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_discountServicesAdmin.GetDiscount());
        }
    }
}
