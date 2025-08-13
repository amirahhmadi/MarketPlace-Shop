using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.ProductServices.ProductServicesAdmin;
using GameOnline.Core.Services.PropertyService.PropertyNameService;
using GameOnline.Core.Services.PropertyService.PropertyValueService;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyValueController : BaseAdminController
    {
        private readonly IPropertyValueServiceAdmin _propertyValueServiceAdmin;
        private readonly IPropertyNameServiceAdmin _propertyNameServiceAdmin;
        private readonly IProductServicesAdmin _productServiceAdmin;

        public PropertyValueController(IPropertyValueServiceAdmin propertyValueServiceAdmin,
                                       IPropertyNameServiceAdmin propertyNameServiceAdmin,
                                       IProductServicesAdmin productServiceAdmin)
        {
            _propertyValueServiceAdmin = propertyValueServiceAdmin;
            _propertyNameServiceAdmin = propertyNameServiceAdmin;
            _productServiceAdmin = productServiceAdmin;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _propertyValueServiceAdmin.GetPropertyValues();
            return View(model);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PropertyNames = _propertyNameServiceAdmin.GetPropertyName();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePropertyValueViewmodel createPropertyValue)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                ViewBag.PropertyNames = _propertyNameServiceAdmin.GetPropertyName();
                return View(createPropertyValue);
            }

            _propertyValueServiceAdmin.CreatePropertyValue(createPropertyValue);
            SetSweetAlert("success", "عملیات موفق", "مقدار ویژگی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _propertyValueServiceAdmin.GetPropertyValueById(id);
            if (result == null)
            {
                SetSweetAlert("error", "خطا", "مقدار ویژگی پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            result.GetPropertyName = _propertyNameServiceAdmin.GetPropertyName();
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyValueViewmodel editPropertyValue)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                editPropertyValue.GetPropertyName = _propertyNameServiceAdmin.GetPropertyName();
                return View(editPropertyValue);
            }

            _propertyValueServiceAdmin.EditPropertyValue(editPropertyValue);
            SetSweetAlert("success", "عملیات موفق", "مقدار ویژگی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpPost]
        public IActionResult Remove(int propertyValueId)
        {
            _propertyValueServiceAdmin.RemovePropertyValue(propertyValueId);
            SetSweetAlert("success", "عملیات موفق", "مقدار ویژگی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ProductProperty
        [HttpGet]
        public IActionResult ProductProperty(int id)
        {
            var product = _productServiceAdmin.GetProductById(id);
            var addOrUpdate = new AddOrUpdatePropertyValueForProductViewmodel
            {
                ProductId = id,
                categoryid = product.CategoryId,
                propertyNameForProduct = _propertyValueServiceAdmin.GetPropertyNameForProductByCategoryId(product.CategoryId)
            };

            ViewBag.OldValue = _propertyValueServiceAdmin.oldPropertyValueForProduct(id);
            return View(addOrUpdate);
        }

        [HttpPost]
        public IActionResult ProductProperty(AddOrUpdatePropertyValueForProductViewmodel addOrUpdateProperty)
        {
            if (addOrUpdateProperty.nameid.Count() != addOrUpdateProperty.value.Count())
            {
                var product = _productServiceAdmin.GetProductById(addOrUpdateProperty.ProductId);
                addOrUpdateProperty.categoryid = product.CategoryId;
                addOrUpdateProperty.propertyNameForProduct = _propertyValueServiceAdmin.GetPropertyNameForProductByCategoryId(product.CategoryId);
                ViewBag.OldValue = _propertyValueServiceAdmin.oldPropertyValueForProduct(addOrUpdateProperty.ProductId);

                SetSweetAlert("error", "خطا", "تعداد نام و مقدار ویژگی‌ها برابر نیست.");
                return View(addOrUpdateProperty);
            }

            _propertyValueServiceAdmin.AddOrRemovePropertyForProduct(addOrUpdateProperty);
            SetSweetAlert("success", "عملیات موفق", "ویژگی‌های محصول با موفقیت به‌روزرسانی شد.");
            return Redirect("/Admin/Product");
        }
        #endregion
    }
}
