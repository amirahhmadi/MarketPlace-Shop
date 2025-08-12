using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin;
using GameOnline.Core.Services.PropertyService.PropertyGroupService;
using GameOnline.Core.Services.PropertyService.PropertyNameService;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.DataBase.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyNameController : BaseAdminController
    {
        private readonly IPropertyNameServiceAdmin _propertyNameServiceAdmin;
        private readonly IPropertyGroupServiceAdmin _propertyGroupServiceAdmin;
        private readonly ICategoryServiceAdmin _categoryServiceAdmin;

        public PropertyNameController(IPropertyNameServiceAdmin propertyNameServiceAdmin, IPropertyGroupServiceAdmin propertyGroupServiceAdmin, ICategoryServiceAdmin categoryServiceAdmin)
        {
            _propertyNameServiceAdmin = propertyNameServiceAdmin;
            _propertyGroupServiceAdmin = propertyGroupServiceAdmin;
            _categoryServiceAdmin = categoryServiceAdmin;
        }

        public IActionResult Index()
        {
            return View(_propertyNameServiceAdmin.GetPropertyName());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Groups = _propertyGroupServiceAdmin.GetPropertyGroups();
            ViewBag.Category = _categoryServiceAdmin.GetCategory();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePropertyNameViewmodel createPropertyName)
        {
            var result = _propertyNameServiceAdmin.CreatePropertyName(createPropertyName);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _propertyNameServiceAdmin.GetPropertyNameById(id);

            if (result == null)
                return NotFound();

            result.GetPropertyGroups = _propertyGroupServiceAdmin.GetPropertyGroups();
            return View(result); 
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyNameViewmodel editPropertyName)
        {
            if (!ModelState.IsValid)
            {
                editPropertyName.GetPropertyGroups = _propertyGroupServiceAdmin.GetPropertyGroups();
                return View(editPropertyName);
            }

            var result = _propertyNameServiceAdmin.EditPropertyName(editPropertyName);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int propertyNameId)
        {
            var result = _propertyNameServiceAdmin.RemovePropertyName(propertyNameId);
            TempData["Result"] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
