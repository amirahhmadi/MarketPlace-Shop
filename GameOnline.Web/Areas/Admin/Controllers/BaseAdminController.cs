using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BaseAdminController : Controller
    {
        public static string Result = "Result";
        public static string ResultParent = "ResultParent";
        public static string ProductId = "ProductId";
    }
}
