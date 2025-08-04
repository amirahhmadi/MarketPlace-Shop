using GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;
using GameOnline.Core.ViewModels.DiscountViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(CreateDiscountViewModels discountViewModels)
        {
            var result = _discountServicesAdmin.CreateDiscount(discountViewModels);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
