using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
