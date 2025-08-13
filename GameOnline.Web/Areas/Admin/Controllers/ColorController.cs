using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.ColorServices.ColorServicesAdmin;
using GameOnline.Core.ViewModels.ColorViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class ColorController : BaseAdminController
    {
        private readonly IColorServicesAdmin _colorServicesAdmin;

        public ColorController(IColorServicesAdmin colorServicesAdmin)
        {
            _colorServicesAdmin = colorServicesAdmin;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_colorServicesAdmin.GetColors());
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateColorsViewModel createColors)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(createColors);
            }

            _colorServicesAdmin.CreateColor(createColors);
            SetSweetAlert("success", "عملیات موفق", "رنگ با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var color = _colorServicesAdmin.GetColorById(id);
            if (color == null)
                return NotFound();

            return View(color);
        }

        [HttpPost]
        public IActionResult Edit(EditColorsViewModel editColors)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(editColors);
            }

            _colorServicesAdmin.EditColor(editColors);
            SetSweetAlert("success", "عملیات موفق", "رنگ با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var color = _colorServicesAdmin.GetColorById(id);
            if (color == null)
                return NotFound();

            var removeModel = new RemoveColorsViewModel
            {
                ColorId = color.ColorId,
                ColorName = color.ColorName,
                ColorCode = color.ColorCode
            };

            return View(removeModel);
        }

        [HttpPost]
        public IActionResult Remove(RemoveColorsViewModel removeColors)
        {
            _colorServicesAdmin.RemoveColor(removeColors);
            SetSweetAlert("success", "عملیات موفق", "رنگ با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
