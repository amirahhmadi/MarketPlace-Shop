using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.BrandServices.BrandServicesAdmin;
using GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin;
using GameOnline.Core.Services.ProductServices.ProductServicesAdmin;
using GameOnline.Core.ViewModels.ProductViewmodel.Admin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IProductServicesAdmin _productServicesAdmin;
        private readonly IBrandServiceAdmin _brandServiceAdmin;
        private readonly ICategoryServiceAdmin _categoryServiceAdmin;

        public ProductController(
            IProductServicesAdmin productServicesAdmin,
            IBrandServiceAdmin brandServiceAdmin,
            ICategoryServiceAdmin categoryServiceAdmin)
        {
            _productServicesAdmin = productServicesAdmin;
            _brandServiceAdmin = brandServiceAdmin;
            _categoryServiceAdmin = categoryServiceAdmin;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _productServicesAdmin.GetProducts();
            return View(model);
        }
        #endregion

        #region Create
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
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
            }
            var result = _productServicesAdmin.CreateProduct(createProduct);
            SetSweetAlert("success", "عملیات موفق", "محصول با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productServicesAdmin.GetProductById(id);
            if (product == null)
            {
                SetSweetAlert("error", "خطا", "محصول پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

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
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(editProduct);
            }

            _productServicesAdmin.EditProduct(editProduct);
            SetSweetAlert("success", "عملیات موفق", "محصول با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var product = _productServicesAdmin.GetProductById(id);
            if (product == null)
            {
                SetSweetAlert("error", "خطا", "محصول پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            product.GetCategories = _categoryServiceAdmin.GetCategory();
            product.GetBrands = _brandServiceAdmin.GetBrands();

            return View(product);
        }

        [HttpPost]
        public IActionResult Remove(RemoveProductViewModel removeProduct)
        {
            _productServicesAdmin.RemoveProduct(removeProduct);
            SetSweetAlert("success", "عملیات موفق", "محصول با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ProductReview
        [HttpGet]
        public IActionResult ProductReview(int id)
        {
            var review = _productServicesAdmin.FindProductReviewById(id) ??
                         new AddOrUpdateProductReviewViewmodel { ProductId = id };
            return View(review);
        }

        [HttpPost]
        public IActionResult ProductReview(AddOrUpdateProductReviewViewmodel reviewViewmodel)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده کامل یا صحیح نیست.");
                return View(reviewViewmodel);
            }

            _productServicesAdmin.EditProductReview(reviewViewmodel);

            SetSweetAlert("success", "عملیات موفق", "بررسی محصول با موفقیت ثبت شد.");
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
