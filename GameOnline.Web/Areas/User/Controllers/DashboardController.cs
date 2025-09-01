using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.User.Controllers
{
    public class DashboardController : BaseUserController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
