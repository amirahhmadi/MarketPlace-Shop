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
                return View();
            }

            var result = _categoryServiceAdmin.CreateCategory(createCategory);

            if (result != null && result.IsSuccess && createCategory.AddOrEditParent != null)
            {
                createCategory.AddOrEditParent.SubId = result.Data;
                var resultParent = _categoryServiceAdmin.AddOrEditParentCategory(createCategory.AddOrEditParent);
                TempData[ResultParent] = JsonConvert.SerializeObject(resultParent);
            }

            TempData[Result] = JsonConvert.SerializeObject(result);
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

            if (result.IsSuccess)
            {
                editCategory.AddOrEditParent = editCategory.AddOrEditParent ?? new AddOrEditParentCategoryViewmodel();
                editCategory.AddOrEditParent.SubId = result.Data;
                var resultParent = _categoryServiceAdmin.AddOrEditParentCategory(editCategory.AddOrEditParent);
                TempData[ResultParent] = JsonConvert.SerializeObject(resultParent);
            }

            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var brand = _categoryServiceAdmin.GetCategoryById(id);

            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        public IActionResult Remove(RemoveCategoriesViewModels removeCategory)
        {
            var result = _categoryServiceAdmin.RemoveCategory(removeCategory);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
