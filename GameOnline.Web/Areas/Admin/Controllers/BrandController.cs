using GameOnline.Core.Services.BrandServices.BrandServicesAdmin;
using GameOnline.Core.ViewModels.BrandViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class BrandController : BaseAdminController
    {
        private readonly IBrandServiceAdmin _brandServiceAdmin;

        public BrandController(IBrandServiceAdmin brandServiceAdmin)
        {
            _brandServiceAdmin = brandServiceAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_brandServiceAdmin.GetBrands());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBrandsViewModel createBrand)
        {
            var result = _brandServiceAdmin.CreateBrand(createBrand);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var brand = _brandServiceAdmin.GetBrandById(id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(EditBrandsViewModel editBrand)
        {
            var result = _brandServiceAdmin.EditBrand(editBrand);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var brand = _brandServiceAdmin.GetBrandById(id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        public IActionResult Remove(RemoveBrandsViewModel removeBrand)
        {
            var result = _brandServiceAdmin.RemoveBrand(removeBrand);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
