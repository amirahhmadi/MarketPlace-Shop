using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.BrandServices.Commands;
using GameOnline.Core.Services.BrandServices.Queries;
using GameOnline.Core.Services.CategoryServices.Commands;
using GameOnline.Core.Services.CategoryServices.Queries;
using GameOnline.Core.Services.ProductServices.Commands;
using GameOnline.Core.Services.ProductServices.Queries;
using GameOnline.Core.ViewModels.ProductViewmodel.Admin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IProductServicesCommand _productCommand;
        private readonly IProductServicesQuery _productQuery;
        private readonly IBrandServiceCommand _brandCommand;
        private readonly IBrandServiceQuery _brandQuery;
        private readonly ICategoryServicesCommand _categoryCommand;
        private readonly ICategoryServicesQuery _categoryQuery;

        public ProductController(IProductServicesCommand productCommand, IProductServicesQuery productQuery, IBrandServiceCommand brandCommand, IBrandServiceQuery brandQuery, ICategoryServicesCommand categoryCommand, ICategoryServicesQuery categoryQuery)
        {
            _productCommand = productCommand;
            _productQuery = productQuery;
            _brandCommand = brandCommand;
            _brandQuery = brandQuery;
            _categoryCommand = categoryCommand;
            _categoryQuery = categoryQuery;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _productQuery.GetProducts();
            return View(model);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            CreateProductViewmodel createProduct = new CreateProductViewmodel();
            createProduct.GetCategories = _categoryQuery.GetCategory();
            createProduct.GetBrands = _brandQuery.GetBrands();
            return View(createProduct);
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewmodel createProduct)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
            }
            var result = _productCommand.CreateProduct(createProduct);
            SetSweetAlert("success", "عملیات موفق", "محصول با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productQuery.GetProductById(id);
            if (product == null)
            {
                SetSweetAlert("error", "خطا", "محصول پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            product.GetCategories = _categoryQuery.GetCategory();
            product.GetBrands = _brandQuery.GetBrands();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewmodel editProduct)
        {
            if (!ModelState.IsValid)
            {
                editProduct.GetCategories = _categoryQuery.GetCategory();
                editProduct.GetBrands = _brandQuery.GetBrands();
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(editProduct);
            }

            _productCommand.EditProduct(editProduct);
            SetSweetAlert("success", "عملیات موفق", "محصول با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var product = _productQuery.GetProductById(id);
            if (product == null)
            {
                SetSweetAlert("error", "خطا", "محصول پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            product.GetCategories = _categoryQuery.GetCategory();
            product.GetBrands = _brandQuery.GetBrands();

            return View(product);
        }

        [HttpPost]
        public IActionResult Remove(RemoveProductViewModel removeProduct)
        {
            _productCommand.RemoveProduct(removeProduct);
            SetSweetAlert("success", "عملیات موفق", "محصول با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ProductReview
        [HttpGet]
        public IActionResult ProductReview(int id)
        {
            var review = _productQuery.FindProductReviewById(id) ??
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

            _productCommand.EditProductReview(reviewViewmodel);

            SetSweetAlert("success", "عملیات موفق", "بررسی محصول با موفقیت ثبت شد.");
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
