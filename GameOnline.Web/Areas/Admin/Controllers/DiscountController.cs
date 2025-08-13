using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;
using GameOnline.Core.ViewModels.DiscountViewModels;
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

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_discountServicesAdmin.GetDiscount());
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDiscountViewModels discountViewModels)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(discountViewModels);
            }

            _discountServicesAdmin.CreateDiscount(discountViewModels);
            SetSweetAlert("success", "عملیات موفق", "تخفیف با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}