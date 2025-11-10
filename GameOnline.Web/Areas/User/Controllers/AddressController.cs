using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GameOnline.Core.Services.AddressService.Queries;

namespace GameOnline.Web.Areas.User.Controllers
{
    public class AddressController : BaseUserController
    {
        private readonly IAddressServiceQuery _addressServiceQuery;

        [Route("GetAddress")]
        public IActionResult GetAddress()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(_addressServiceQuery.GetAddressForProfile(userId));
        }
    }
}
