using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class BaseController : Controller
    {
        public const string ProductEn = "ProductEn";
        public void SetSweetAlert(string type, string title, string message)
        {
            TempData["SwalType"] = type;
            TempData["SwalTitle"] = title;
            TempData["SwalMessage"] = message;
        }
    }
}
