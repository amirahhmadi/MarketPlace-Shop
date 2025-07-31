using GameOnline.Core.Services.ProductServices.ProductServicesAdmin;
using GameOnline.Core.Services.PropertyService.PropertyNameService;
using GameOnline.Core.Services.PropertyService.PropertyValueService;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyValueController : BaseAdminController
    {

        #region ctor
        private readonly IPropertyValueServiceAdmin _propertyValueServiceAdmin;
        private readonly IPropertyNameServiceAdmin _propertyNameServiceAdmin;
        private readonly IProductServicesAdmin _productServiceAdmin;
        public PropertyValueController(IPropertyValueServiceAdmin propertyValueServiceAdmin, IPropertyNameServiceAdmin propertyNameServiceAdmin, IProductServicesAdmin productServiceAdmin)
        {
            _propertyValueServiceAdmin = propertyValueServiceAdmin;
            _propertyNameServiceAdmin = propertyNameServiceAdmin;
            _productServiceAdmin = productServiceAdmin;
        }

        #endregion

        #region Index
        public IActionResult Index()
        {
            return View(_propertyValueServiceAdmin.GetPropertyValues());
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
            var result = _propertyValueServiceAdmin.CreatePropertyValue(createPropertyValue);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _propertyValueServiceAdmin.GetPropertyValueById(id);

            if (result == null)
                return NotFound();

            result.GetPropertyName = _propertyNameServiceAdmin.GetPropertyName();
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyValueViewmodel editPropertyValue)
        {
            if (!ModelState.IsValid)
            {
                editPropertyValue.GetPropertyName = _propertyNameServiceAdmin.GetPropertyName();
                return View(editPropertyValue);
            }

            var result = _propertyValueServiceAdmin.EditPropertyValue(editPropertyValue);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int propertyValueId)
        {
            var result = _propertyValueServiceAdmin.RemovePropertyValue(propertyValueId);
            TempData["Result"] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
