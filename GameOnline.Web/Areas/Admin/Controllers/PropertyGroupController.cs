using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.PropertyService.Commands.PropertyGroup;
using GameOnline.Core.Services.PropertyService.Queries.PropertyGroup;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyGroupController : BaseAdminController
    {
        private readonly IPropertyGroupCommand _groupCommand;
        private readonly IPropertyGroupQuery _groupQuery;

        public PropertyGroupController(IPropertyGroupCommand groupCommand, IPropertyGroupQuery groupQuery)
        {
            _groupCommand = groupCommand;
            _groupQuery = groupQuery;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _groupQuery.GetPropertyGroups();
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

            _groupCommand.CreatePropertyGroup(createPropertyGroup);
            SetSweetAlert("success", "عملیات موفق", "گروه ویژگی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var propertyGroup = _groupQuery.GetPropertyGroupById(id);
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

            _groupCommand.EditPropertyGroup(editPropertyGroup);
            SetSweetAlert("success", "عملیات موفق", "گروه ویژگی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpPost]
        public IActionResult Remove(int propertyGroupId)
        {
            _groupCommand.RemovePropertyGroup(propertyGroupId);
            SetSweetAlert("success", "عملیات موفق", "گروه ویژگی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
