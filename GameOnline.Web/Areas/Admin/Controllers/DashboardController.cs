using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        #region Index

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
