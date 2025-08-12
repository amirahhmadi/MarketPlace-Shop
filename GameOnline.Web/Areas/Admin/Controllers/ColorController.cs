using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.ColorServices.ColorServicesAdmin;
using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class ColorController : BaseAdminController
    {
        private readonly IColorServicesAdmin _colorServicesAdmin;

        public ColorController(IColorServicesAdmin colorServicesAdmin)
        {
            _colorServicesAdmin = colorServicesAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_colorServicesAdmin.GetColors());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateColorsViewModel createColors)
        {
            var result = _colorServicesAdmin.CreateColor(createColors);

            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);

            return RedirectToAction(nameof(Index));
        }

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
            var result = _colorServicesAdmin.EditColor(editColors);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
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

            return View(removeModel); // ✅ حالا ویو مدل درستی می‌گیره
        }

        [HttpPost]
        public IActionResult Remove(RemoveColorsViewModel removeColors)
        {
            var result = _colorServicesAdmin.RemoveColor(removeColors);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
