using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.CategoryServices.CategoryServicesAdmin;
using GameOnline.Core.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryServiceAdmin _categoryServiceAdmin;
        public CategoryController(ICategoryServiceAdmin categoryServicesAdmin)
        {
            _categoryServiceAdmin = categoryServicesAdmin;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_categoryServiceAdmin.GetCategory());
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ParentList = _categoryServiceAdmin.GetCategoriesForParentList(0);
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoriesViewModels createCategory)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ParentList = _categoryServiceAdmin.GetCategoriesForParentList(0);
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View();
            }

            var result = _categoryServiceAdmin.CreateCategory(createCategory);

            if (result != null && createCategory.AddOrEditParent != null)
            {
                createCategory.AddOrEditParent.SubId = result.Data;
                var resultParent = _categoryServiceAdmin.AddOrEditParentCategory(createCategory.AddOrEditParent);
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
            var category = _categoryServiceAdmin.GetCategoryById(id);
            if (category == null)
                return NotFound();

            ViewBag.Parents = _categoryServiceAdmin.GetParentCategoryForAddOrRemoveSub(id);
            ViewBag.ParentList = _categoryServiceAdmin.GetCategoriesForParentList(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(EditCategoriesViewModels editCategory)
        {
            var result = _categoryServiceAdmin.EditCategory(editCategory);

            if (result != null && editCategory.AddOrEditParent != null)
            {
                editCategory.AddOrEditParent.SubId = result.Data;
                var resultParent = _categoryServiceAdmin.AddOrEditParentCategory(editCategory.AddOrEditParent);
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
            var category = _categoryServiceAdmin.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Remove(RemoveCategoriesViewModels removeCategory)
        {
            var result = _categoryServiceAdmin.RemoveCategory(removeCategory);
            SetSweetAlert("success", "عملیات موفق", "دسته‌بندی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
