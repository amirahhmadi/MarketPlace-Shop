using GameOnline.Core.Services.GuaranteeServices.GuaranteeServicesAdmin;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class GuaranteeController : BaseAdminController
    {
        private readonly IGuaranteeServiceAdmin _guaranteeServiceAdmin;
        public GuaranteeController(IGuaranteeServiceAdmin guaranteeServiceAdmin)
        {
            _guaranteeServiceAdmin = guaranteeServiceAdmin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_guaranteeServiceAdmin.GetGuarantees());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateGuaranteesViewModel createGuarantee)
        {
            var result = _guaranteeServiceAdmin.CreateGuarantee(createGuarantee);

            TempData[Result] = JsonConvert.SerializeObject(result);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var guarantee = _guaranteeServiceAdmin.GetGuaranteeById(id);

            if (guarantee == null)
                return NotFound();

            return View(guarantee);
        }

        [HttpPost]
        public IActionResult Edit(EditGuaranteesViewModel editGuarantee)
        {
            var result = _guaranteeServiceAdmin.EditGuarantee(editGuarantee);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var guarantee = _guaranteeServiceAdmin.GetGuaranteeById(id);

            if (guarantee == null)
                return NotFound();

            return View(guarantee);
        }

        [HttpPost]
        public IActionResult Remove(RemoveGuaranteesViewModel removeGuarantees)
        {
            var result = _guaranteeServiceAdmin.RemoveGuarantee(removeGuarantees);
            TempData[Result] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index));
        }
    }
}
