using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.CategoryServices.Commands;
using GameOnline.Core.Services.CategoryServices.Queries;
using GameOnline.Core.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryServicesCommand _servicesCommand;
        private readonly ICategoryServicesQuery _servicesQuery;

        public CategoryController(ICategoryServicesCommand servicesCommand, ICategoryServicesQuery servicesQuery)
        {
            _servicesCommand = servicesCommand;
            _servicesQuery = servicesQuery;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_servicesQuery.GetCategory());
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ParentList = _servicesQuery.GetCategoriesForParentList(0);
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoriesViewModels createCategory)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentList = _servicesQuery.GetCategoriesForParentList(0);
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View();
            }

            var result = _servicesCommand.CreateCategory(createCategory);

            if (result != null && createCategory.AddOrEditParent != null)
            {
                createCategory.AddOrEditParent.SubId = result.Data;
                var resultParent = _servicesCommand.AddOrEditParentCategory(createCategory.AddOrEditParent);
                TempData[TempDataName.ResultParent] = JsonConvert.SerializeObject(resultParent);
            }

            SetSweetAlert("success", "عملیات موفق", "دسته‌بندی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _servicesQuery.GetCategoryById(id);
            if (category == null)
                return NotFound();

            ViewBag.Parents = _servicesQuery.GetParentCategoryForAddOrRemoveSub(id);
            ViewBag.ParentList = _servicesQuery.GetCategoriesForParentList(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(EditCategoriesViewModels editCategory)
        {
            var result = _servicesCommand.EditCategory(editCategory);

            if (result != null && editCategory.AddOrEditParent != null)
            {
                editCategory.AddOrEditParent.SubId = result.Data;
                var resultParent = _servicesCommand.AddOrEditParentCategory(editCategory.AddOrEditParent);
                TempData[TempDataName.ResultParent] = JsonConvert.SerializeObject(resultParent);
            }

            SetSweetAlert("success", "عملیات موفق", "دسته‌بندی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var category = _servicesQuery.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Remove(RemoveCategoriesViewModels removeCategory)
        {
            var result = _servicesCommand.RemoveCategory(removeCategory);
            SetSweetAlert("success", "عملیات موفق", "دسته‌بندی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
