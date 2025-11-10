using GameOnline.Core.Services.UserService.Commands;
using GameOnline.Core.Services.UserService.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IUserServiceCommand _serviceCommand;
        private readonly IUserServiceQuery _serviceQuery;

        public UserController(IUserServiceCommand serviceCommand, IUserServiceQuery serviceQuery)
        {
            _serviceCommand = serviceCommand;
            _serviceQuery = serviceQuery;
        }
        public IActionResult Index()
        {
            return View(_serviceQuery.GetUsers());
        }
    }
}
