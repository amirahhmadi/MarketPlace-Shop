using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.PropertyService.PropertyGroupService;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyGroupController : BaseAdminController
    {
        private readonly IPropertyGroupServiceAdmin _propertyGroupServiceAdmin;
        public PropertyGroupController(IPropertyGroupServiceAdmin propertyGroupServiceAdmin)
        {
            _propertyGroupServiceAdmin = propertyGroupServiceAdmin;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _propertyGroupServiceAdmin.GetPropertyGroups();
            return View(model);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePropertyGroupsViewmodel createPropertyGroup)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(createPropertyGroup);
            }

            _propertyGroupServiceAdmin.CreatePropertyGroup(createPropertyGroup);
            SetSweetAlert("success", "عملیات موفق", "گروه ویژگی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var propertyGroup = _propertyGroupServiceAdmin.GetPropertyGroupById(id);
            if (propertyGroup == null)
            {
                SetSweetAlert("error", "خطا", "گروه ویژگی پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            return View(propertyGroup);
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyGroupsViewmodel editPropertyGroup)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(editPropertyGroup);
            }

            _propertyGroupServiceAdmin.EditPropertyGroup(editPropertyGroup);
            SetSweetAlert("success", "عملیات موفق", "گروه ویژگی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpPost]
        public IActionResult Remove(int propertyGroupId)
        {
            _propertyGroupServiceAdmin.RemovePropertyGroup(propertyGroupId);
            SetSweetAlert("success", "عملیات موفق", "گروه ویژگی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
