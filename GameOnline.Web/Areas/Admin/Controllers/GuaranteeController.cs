using GameOnline.Core.Services.GuaranteeServices.Commands;
using GameOnline.Core.Services.GuaranteeServices.Queries;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class GuaranteeController : BaseAdminController
    {
        private readonly IGuaranteeServiceCommand _serviceCommand;
        private readonly IGuaranteeServiceQuery _serviceQuery;

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _serviceQuery.GetGuarantees();
            return View(model);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateGuaranteesViewModel createGuarantee)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(createGuarantee);
            }

            _serviceCommand.CreateGuarantee(createGuarantee);
            SetSweetAlert("success", "عملیات موفق", "گارانتی با موفقیت ایجاد شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var guarantee = _serviceQuery.GetGuaranteeById(id);
            if (guarantee == null)
            {
                SetSweetAlert("error", "خطا", "گارانتی پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }
            return View(guarantee);
        }

        [HttpPost]
        public IActionResult Edit(EditGuaranteesViewModel editGuarantee)
        {
            if (!ModelState.IsValid)
            {
                SetSweetAlert("error", "خطا", "اطلاعات وارد شده صحیح نیست.");
                return View(editGuarantee);
            }

            _serviceCommand.EditGuarantee(editGuarantee);
            SetSweetAlert("success", "عملیات موفق", "گارانتی با موفقیت ویرایش شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var guarantee = _serviceQuery.GetGuaranteeById(id);
            if (guarantee == null)
            {
                SetSweetAlert("error", "خطا", "گارانتی پیدا نشد.");
                return RedirectToAction(nameof(Index));
            }
            return View(guarantee);
        }

        [HttpPost]
        public IActionResult Remove(RemoveGuaranteesViewModel removeGuarantees)
        {
            _serviceCommand.RemoveGuarantee(removeGuarantees);
            SetSweetAlert("success", "عملیات موفق", "گارانتی با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
