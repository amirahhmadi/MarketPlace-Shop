using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class BaseUserController : Controller
    {
    }
}
