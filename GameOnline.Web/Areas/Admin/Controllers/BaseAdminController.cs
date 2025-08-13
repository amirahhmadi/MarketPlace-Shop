using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BaseAdminController : Controller
    {
        public void SetSweetAlert(string type, string title, string message)
        {
            TempData["SwalType"] = type;
            TempData["SwalTitle"] = title;
            TempData["SwalMessage"] = message;
        }
    }
}
