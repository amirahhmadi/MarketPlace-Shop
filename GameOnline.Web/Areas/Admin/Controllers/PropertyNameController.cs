using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin;
using GameOnline.Core.Services.PropertyService.PropertyGroupService;
using GameOnline.Core.Services.PropertyService.PropertyNameService;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyNameController : BaseAdminController
    {
        private readonly IPropertyNameServiceAdmin _propertyNameServiceAdmin;
        private readonly IPropertyGroupServiceAdmin _propertyGroupServiceAdmin;
        private readonly ICategoryServiceAdmin _categoryServiceAdmin;

        public PropertyNameController(IPropertyNameServiceAdmin propertyNameServiceAdmin,
                                      IPropertyGroupServiceAdmin propertyGroupServiceAdmin,
                                      ICategoryServiceAdmin categoryServiceAdmin)
        {
            _propertyNameServiceAdmin = propertyNameServiceAdmin;
            _propertyGroupServiceAdmin = propertyGroupServiceAdmin;
            _categoryServiceAdmin = categoryServiceAdmin;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _propertyNameServiceAdmin.GetPropertyName();
            return View(model);
        }
        #endregion

        #region Create
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
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                ViewBag.Groups = _propertyGroupServiceAdmin.GetPropertyGroups();
                ViewBag.Category = _categoryServiceAdmin.GetCategory();
                return View(createPropertyName);
            }

            _propertyNameServiceAdmin.CreatePropertyName(createPropertyName);
            SetSweetAlert("success", "عملیات موفق", "ویژگی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _propertyNameServiceAdmin.GetPropertyNameById(id);
            if (result == null)
            {
                SetSweetAlert("error", "خطا", "ویژگی پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            result.GetPropertyGroups = _propertyGroupServiceAdmin.GetPropertyGroups();
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyNameViewmodel editPropertyName)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                editPropertyName.GetPropertyGroups = _propertyGroupServiceAdmin.GetPropertyGroups();
                return View(editPropertyName);
            }

            _propertyNameServiceAdmin.EditPropertyName(editPropertyName);
            SetSweetAlert("success", "عملیات موفق", "ویژگی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpPost]
        public IActionResult Remove(int propertyNameId)
        {
            _propertyNameServiceAdmin.RemovePropertyName(propertyNameId);
            SetSweetAlert("success", "عملیات موفق", "ویژگی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
