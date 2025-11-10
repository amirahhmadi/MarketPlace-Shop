using GameOnline.Core.Services.GalleryServices.Commands;
using GameOnline.Core.Services.GalleryServices.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class GalleryController : BaseAdminController
    {
        private readonly IGalleryServicesCommand _servicesCommand;
        private readonly IGalleryServicesQuery _servicesQuery;
        #region Index
        [HttpGet]
        public IActionResult Index(int id)
        {
            TempData["ProductId"] = id;
            ViewBag.ProductId = id;
            var model = _servicesQuery.GetImageGalleryForProductById(id);
            return View(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public IActionResult Create(int productId, IFormFile imageName)
        {
            if (imageName == null)
            {
                SetSweetAlert("error", "خطا", "لطفاً یک تصویر انتخاب کنید.");
                return RedirectToAction(nameof(Index), new { id = productId });
            }

            _servicesCommand.CreateImageForGallery(productId, imageName);
            SetSweetAlert("success", "عملیات موفق", "تصویر با موفقیت اضافه شد.");
            return RedirectToAction(nameof(Index), new { id = productId });
        }
        #endregion

        #region Remove
        [HttpPost]
        public IActionResult Remove(int galleryId, int productId)
        {
            _servicesCommand.RemoveImageFromGallery(galleryId);
            SetSweetAlert("success", "عملیات موفق", "تصویر با موفقیت حذف شد.");
            return RedirectToAction(nameof(Index), new { id = productId });
        }
        #endregion
    }
}