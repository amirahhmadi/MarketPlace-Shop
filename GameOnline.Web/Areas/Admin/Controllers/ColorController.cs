using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.ColorServices.Commands;
using GameOnline.Core.Services.ColorServices.Queries;
using GameOnline.Core.ViewModels.ColorViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class ColorController : BaseAdminController
    {
        private readonly IColorServicesCommand _servicesCommand;
        private readonly IColorServicesQuery _servicesQuery;

        public ColorController(IColorServicesCommand servicesCommand, IColorServicesQuery servicesQuery)
        {
            _servicesCommand = servicesCommand;
            _servicesQuery = servicesQuery;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View(_servicesQuery.GetColors());
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

            _servicesCommand.CreateColor(createColors);
            SetSweetAlert("success", "عملیات موفق", "رنگ با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var color = _servicesQuery.GetColorById(id);
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

            _servicesCommand.EditColor(editColors);
            SetSweetAlert("success", "عملیات موفق", "رنگ با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var color = _servicesQuery.GetColorById(id);
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
            _servicesCommand.RemoveColor(removeColors);
            SetSweetAlert("success", "عملیات موفق", "رنگ با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
