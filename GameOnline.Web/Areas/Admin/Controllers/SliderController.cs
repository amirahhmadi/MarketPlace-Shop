using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.SliderServices.Commands;
using GameOnline.Core.Services.SliderServices.Queries;
using GameOnline.Core.ViewModels.SliderViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class SliderController : BaseAdminController
    {
        private readonly ISliderServiceCommand _serviceCommand;
        private readonly ISliderServiceQuery _serviceQuery;

        public SliderController(ISliderServiceCommand serviceCommand, ISliderServiceQuery serviceQuery)
        {
            _serviceCommand = serviceCommand;
            _serviceQuery = serviceQuery;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _serviceQuery.GetSliders();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSlidersViewModel createSlider)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(createSlider);
            }

            _serviceCommand.CreateSlider(createSlider);
            SetSweetAlert("success", "عملیات موفق", "اسلایدر با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var slider = _serviceQuery.GetSliderById(id);

            if (slider == null)
            {
                SetSweetAlert("error", "خطا", "اسلایدر پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(EditSlidersViewModel editSlider)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(editSlider);
            }

            _serviceCommand.EditSlider(editSlider);
            SetSweetAlert("success", "عملیات موفق", "اسلایدر با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var slider = _serviceQuery.GetSliderById(id);

            if (slider == null)
            {
                SetSweetAlert("error", "خطا", "اسلایدر پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }

            return View(slider);
        }

        [HttpPost]
        public IActionResult Remove(RemoveSlidersViewModel removeSlider)
        {
            _serviceCommand.RemoveSlider(removeSlider);
            SetSweetAlert("success", "عملیات موفق", "اسلایدر با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
    }
}
