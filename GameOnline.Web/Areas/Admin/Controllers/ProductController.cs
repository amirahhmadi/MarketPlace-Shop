using GameOnline.Core.Services.BrandServices.BrandServicesAdmin;
using GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin;
using GameOnline.Core.Services.ProductServices.ProductServicesAdmin;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.Core.ViewModels.ProductViewmodel;
using GameOnline.Core.ViewModels.SliderViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IProductServicesAdmin _productServicesAdmin;
        private readonly IBrandServiceAdmin _brandServiceAdmin;
        private readonly ICategoryServiceAdmin _categoryServiceAdmin;

        public ProductController(IProductServicesAdmin productServicesAdmin, IBrandServiceAdmin brandServiceAdmin, ICategoryServiceAdmin categoryServiceAdmin)
        {
            _productServicesAdmin = productServicesAdmin;
            _brandServiceAdmin = brandServiceAdmin;
            _categoryServiceAdmin = categoryServiceAdmin;
        }
        public IActionResult Index()
        {
            return View(_productServicesAdmin.GetProducts());
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateProductViewmodel createProduct = new CreateProductViewmodel();
            createProduct.GetCategories = _categoryServiceAdmin.GetCategory();
            createProduct.GetBrands = _brandServiceAdmin.GetBrands();
            return View(createProduct);
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewmodel createProduct)
        {
            var result = _productServicesAdmin.CreateProduct(createProduct);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productServicesAdmin.GetProductById(id);

            if (product == null)
                return NotFound();

            product.GetCategories = _categoryServiceAdmin.GetCategory();
            product.GetBrands = _brandServiceAdmin.GetBrands();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewmodel editProduct)
        {
            if (!ModelState.IsValid)
            {
                editProduct.GetCategories = _categoryServiceAdmin.GetCategory();
                editProduct.GetBrands = _brandServiceAdmin.GetBrands();
                return View(editProduct);
            }

            var result = _productServicesAdmin.EditProduct(editProduct);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var product = _productServicesAdmin.GetProductById(id);

            if (product == null)
                return NotFound();

            product.GetCategories = _categoryServiceAdmin.GetCategory();
            product.GetBrands = _brandServiceAdmin.GetBrands();

            return View(product);
        }

        [HttpPost]
        public IActionResult Remove(RemoveProductViewModel removeProduct)
        {
            var result = _productServicesAdmin.RemoveProduct(removeProduct);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
