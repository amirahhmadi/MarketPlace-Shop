using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.CategoryServices.Commands;
using GameOnline.Core.Services.CategoryServices.Queries;
using GameOnline.Core.Services.PropertyService.Commands.PropertyGroup;
using GameOnline.Core.Services.PropertyService.Commands.PropertyName;
using GameOnline.Core.Services.PropertyService.Queries.PropertyGroup;
using GameOnline.Core.Services.PropertyService.Queries.PropertyName;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class PropertyNameController : BaseAdminController
    {
        private readonly IPropertyNameCommand _propertyNameCommand;
        private readonly IPropertyNameQuery _propertyNameQuery;
        private readonly IPropertyGroupCommand _propertyGroupCommand;
        private readonly IPropertyGroupQuery _propertyGroupQuery;
        private readonly ICategoryServicesCommand _categoryServicesCommand;
        private readonly ICategoryServicesQuery _categoryServicesQuery;

        public PropertyNameController(IPropertyNameCommand propertyNameCommand, IPropertyNameQuery propertyNameQuery, IPropertyGroupCommand propertyGroupCommand, IPropertyGroupQuery propertyGroupQuery, ICategoryServicesCommand categoryServicesCommand, ICategoryServicesQuery categoryServicesQuery)
        {
            _propertyNameCommand = propertyNameCommand;
            _propertyNameQuery = propertyNameQuery;
            _propertyGroupCommand = propertyGroupCommand;
            _propertyGroupQuery = propertyGroupQuery;
            _categoryServicesCommand = categoryServicesCommand;
            _categoryServicesQuery = categoryServicesQuery;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _propertyNameQuery.GetPropertyName();
            return View(model);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Groups = _propertyGroupQuery.GetPropertyGroups();
            ViewBag.Category = _categoryServicesQuery.GetCategory();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatePropertyNameViewmodel createPropertyName)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                ViewBag.Groups = _propertyGroupQuery.GetPropertyGroups();
                ViewBag.Category = _categoryServicesQuery.GetCategory();
                return View(createPropertyName);
            }

            _propertyNameCommand.CreatePropertyName(createPropertyName);
            SetSweetAlert("success", "عملیات موفق", "ویژگی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _propertyNameQuery.GetPropertyNameById(id);
            if (result == null)
            {
                SetSweetAlert("error", "خطا", "ویژگی پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            result.GetPropertyGroups = _propertyGroupQuery.GetPropertyGroups();
            ViewBag.AllCategories = _categoryServicesQuery.GetCategory(); // همه دسته‌ها برای نمایش

            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(EditPropertyNameViewmodel editPropertyName)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                editPropertyName.GetPropertyGroups = _propertyGroupQuery.GetPropertyGroups();
                ViewBag.AllCategories = _categoryServicesQuery.GetCategory();
                return View(editPropertyName);
            }

            var result = _propertyNameCommand.EditPropertyName(editPropertyName);

            if (!result.IsSuccess)
            {
                SetSweetAlert("error", "خطا", "ویرایش ویژگی با مشکل مواجه شد.");
                editPropertyName.GetPropertyGroups = _propertyGroupQuery.GetPropertyGroups();
                ViewBag.AllCategories = _categoryServicesQuery.GetCategory();
                return View(editPropertyName);
            }

            SetSweetAlert("success", "عملیات موفق", "ویژگی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpPost]
        public IActionResult Remove(int propertyNameId)
        {
            _propertyNameCommand.RemovePropertyName(propertyNameId);
            SetSweetAlert("success", "عملیات موفق", "ویژگی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
