using GameOnline.Core.Services.GalleryServices.GalleryServicesAdmin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class GalleryController : BaseAdminController
    {
        private readonly IGalleryServicesAdmin _galleryServicesAdmin;

        public GalleryController(IGalleryServicesAdmin galleryServicesAdmin)
        {
            _galleryServicesAdmin = galleryServicesAdmin;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            TempData["ProductId"] = id;
            ViewBag.ProductId = id;
            var model = _galleryServicesAdmin.GetImageGalleryForProductById(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(int productId, IFormFile imageName)
        {
            var result = _galleryServicesAdmin.CreateImageForGallery(productId, imageName);
            TempData["Result"] = JsonConvert.SerializeObject(result);
            return RedirectToAction(nameof(Index), new { id = productId });
        }

        [HttpPost]
        public IActionResult Remove(int galleryId, int productId)
        {
            var result = _galleryServicesAdmin.RemoveImageFromGallery(galleryId);

            TempData["Result"] = JsonConvert.SerializeObject(result);

            return RedirectToAction(nameof(Index), new { id = productId });
        }
    }
}