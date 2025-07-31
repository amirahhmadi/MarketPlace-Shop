using GameOnline.Core.Services.PropertyService.PropertyGroupService;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyGroupController : BaseAdminController
    {
        private readonly IPropertyGroupServiceAdmin _propertyGroupServiceAdmin;
        public PropertyGroupController(IPropertyGroupServiceAdmin propertyGroupServiceAdmin)
        {
            _propertyGroupServiceAdmin = propertyGroupServiceAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_propertyGroupServiceAdmin.GetPropertyGroups());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePropertyGroupsViewmodel createPropertyGroup)
        {
            var result = _propertyGroupServiceAdmin.CreatePropertyGroup(createPropertyGroup);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var propertyGroup = _propertyGroupServiceAdmin.GetPropertyGroupById(id);

            if (propertyGroup == null)
                return NotFound();

            return View(propertyGroup);
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyGroupsViewmodel editPropertyGroup)
        {
            var result = _propertyGroupServiceAdmin.EditPropertyGroup(editPropertyGroup);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int propertyGroupId)
        {
            var result = _propertyGroupServiceAdmin.RemovePropertyGroup(propertyGroupId);
            TempData["Result"] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
