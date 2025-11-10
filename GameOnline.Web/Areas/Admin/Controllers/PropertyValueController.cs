using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.ProductServices.Commands;
using GameOnline.Core.Services.ProductServices.Queries;
using GameOnline.Core.Services.PropertyService.Commands.PropertyName;
using GameOnline.Core.Services.PropertyService.Commands.PropertyValue;
using GameOnline.Core.Services.PropertyService.Queries.PropertyName;
using GameOnline.Core.Services.PropertyService.Queries.PropertyValue;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyValueController : BaseAdminController
    {
        private readonly IPropertyValueQuery _valueQuery;
        private readonly IPropertyValueCommand _valueCommand;
        private readonly IPropertyNameQuery _nameQuery;
        private readonly IPropertyNameCommand _nameCommand;
        private readonly IProductServicesQuery _productQuery;
        private readonly IProductServicesCommand _productCommand;

        public PropertyValueController(IPropertyValueQuery valueQuery, IPropertyValueCommand valueCommand, IPropertyNameQuery nameQuery, IPropertyNameCommand nameCommand, IProductServicesQuery productQuery, IProductServicesCommand productCommand)
        {
            _valueQuery = valueQuery;
            _valueCommand = valueCommand;
            _nameQuery = nameQuery;
            _nameCommand = nameCommand;
            _productQuery = productQuery;
            _productCommand = productCommand;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _valueQuery.GetPropertyValues();
            return View(model);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PropertyNames = _nameQuery.GetPropertyName();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePropertyValueViewmodel createPropertyValue)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                ViewBag.PropertyNames = _nameQuery.GetPropertyName();
                return View(createPropertyValue);
            }

            _valueCommand.CreatePropertyValue(createPropertyValue);
            SetSweetAlert("success", "عملیات موفق", "مقدار ویژگی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _valueQuery.GetPropertyValueById(id);
            if (result == null)
            {
                SetSweetAlert("error", "خطا", "مقدار ویژگی پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            result.GetPropertyName = _nameQuery.GetPropertyName();
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyValueViewmodel editPropertyValue)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                editPropertyValue.GetPropertyName = _nameQuery.GetPropertyName();
                return View(editPropertyValue);
            }

            _valueCommand.EditPropertyValue(editPropertyValue);
            SetSweetAlert("success", "عملیات موفق", "مقدار ویژگی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpPost]
        public IActionResult Remove(int propertyValueId)
        {
            _valueCommand.RemovePropertyValue(propertyValueId);
            SetSweetAlert("success", "عملیات موفق", "مقدار ویژگی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region ProductProperty
        [HttpGet]
        public IActionResult ProductProperty(int id)
        {
            var product = _productQuery.GetProductById(id);
            var addOrUpdate = new AddOrUpdatePropertyValueForProductViewmodel
            {
                ProductId = id,
                categoryid = product.CategoryId,
                propertyNameForProduct = _valueQuery.GetPropertyNameForProductByCategoryId(product.CategoryId)
            };

            ViewBag.OldValue = _valueQuery.oldPropertyValueForProduct(id);
            return View(addOrUpdate);
        }

        [HttpPost]
        public IActionResult ProductProperty(AddOrUpdatePropertyValueForProductViewmodel addOrUpdateProperty)
        {
            if (addOrUpdateProperty.nameid.Count() != addOrUpdateProperty.value.Count())
            {
                var product = _productQuery.GetProductById(addOrUpdateProperty.ProductId);
                addOrUpdateProperty.categoryid = product.CategoryId;
                addOrUpdateProperty.propertyNameForProduct = _valueQuery.GetPropertyNameForProductByCategoryId(product.CategoryId);
                ViewBag.OldValue = _valueQuery.oldPropertyValueForProduct(addOrUpdateProperty.ProductId);

                SetSweetAlert("error", "خطا", "تعداد نام و مقدار ویژگی‌ها برابر نیست.");
                return View(addOrUpdateProperty);
            }

            _valueCommand.AddOrRemovePropertyForProduct(addOrUpdateProperty);
            SetSweetAlert("success", "عملیات موفق", "ویژگی‌های محصول با موفقیت به‌روزرسانی شد.");
            return Redirect("/Admin/Product");
        }
        #endregion
    }
}
