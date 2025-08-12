using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.SliderServices.SliderServicesAdmin;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.SliderViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class SliderController : BaseAdminController
    {
        private readonly ISliderServiceAdmin _sliderServiceAdmin;

        public SliderController(ISliderServiceAdmin sliderServiceAdmin)
        {
            _sliderServiceAdmin = sliderServiceAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_sliderServiceAdmin.GetSliders());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSlidersViewModel createSlider)
        {
            var result = _sliderServiceAdmin.CreateSlider(createSlider);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var slider = _sliderServiceAdmin.GetSliderById(id);

            if (slider == null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(EditSlidersViewModel editSlider)
        {
            var result = _sliderServiceAdmin.EditSlider(editSlider);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var slider = _sliderServiceAdmin.GetSliderById(id);

            if (slider == null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        public IActionResult Remove(RemoveSlidersViewModel removeSlider)
        {
            var result = _sliderServiceAdmin.RemoveSlider(removeSlider);
            TempData[TempDataName.Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
