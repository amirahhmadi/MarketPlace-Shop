using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.BrandServices.Commands;
using GameOnline.Core.Services.BrandServices.Queries;
using GameOnline.Core.ViewModels.BrandViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class BrandController : BaseAdminController
    {
        private readonly IBrandServiceQuery _serviceQuery;
        private readonly IBrandServiceCommand _serviceCommand;

        public BrandController(IBrandServiceQuery serviceQuery, IBrandServiceCommand serviceCommand)
        {
            _serviceQuery = serviceQuery;
            _serviceCommand = serviceCommand;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_serviceQuery.GetBrands());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBrandsViewModel createBrand)
        {
            var result = _serviceCommand.CreateBrand(createBrand);

            TempData["SwalType"] = result.IsSuccess ? "success" : "error";
            TempData["SwalTitle"] = result.IsSuccess ? "عملیات موفق" : "خطا";
            TempData["SwalMessage"] = result.Message ?? (result.IsSuccess ? "برند با موفقیت ایجاد شد" : "ایجاد برند ناموفق بود");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var brand = _serviceQuery.GetBrandById(id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(EditBrandsViewModel editBrand)
        {
            var result = _serviceCommand.EditBrand(editBrand);

            TempData["SwalType"] = result.IsSuccess ? "success" : "error";
            TempData["SwalTitle"] = result.IsSuccess ? "عملیات موفق" : "خطا";
            TempData["SwalMessage"] = result.Message ?? (result.IsSuccess ? "برند با موفقیت ویرایش شد" : "ویرایش برند ناموفق بود");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var brand = _serviceQuery.GetBrandById(id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        public IActionResult Remove(RemoveBrandsViewModel removeBrand)
        {
            var result = _serviceCommand.RemoveBrand(removeBrand);

            TempData["SwalType"] = result.IsSuccess ? "success" : "error";
            TempData["SwalTitle"] = result.IsSuccess ? "عملیات موفق" : "خطا";
            TempData["SwalMessage"] = result.Message ?? (result.IsSuccess ? "برند با موفقیت حذف شد" : "حذف برند ناموفق بود");

            return RedirectToAction(nameof(Index));
        }
    }
}
