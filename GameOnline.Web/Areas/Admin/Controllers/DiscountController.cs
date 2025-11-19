using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.DiscountServices.Commands;
using GameOnline.Core.Services.DiscountServices.Queries;
using GameOnline.Core.ViewModels.DiscountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class DiscountController : BaseAdminController
    {
        private readonly IDiscountServicesCommand _servicesCommand;
        private readonly IDiscountServicesQuery _servicesQuery;

        public DiscountController(IDiscountServicesCommand servicesCommand, IDiscountServicesQuery servicesQuery)
        {
            _servicesCommand = servicesCommand;
            _servicesQuery = servicesQuery;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_servicesQuery.GetDiscount());
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

            _servicesCommand.CreateDiscount(discountViewModels);
            SetSweetAlert("success", "عملیات موفق", "تخفیف با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}