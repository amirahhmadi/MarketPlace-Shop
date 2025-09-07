using GameOnline.Core.Services.AddressService.AddressServiceClient;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameOnline.Web.Areas.User.Controllers
{
    public class AddressController : BaseUserController
    {
        private readonly IAddressServiceClient _addressServiceClient;

        public AddressController(IAddressServiceClient addressServiceClient)
        {
            _addressServiceClient = addressServiceClient;
        }

        [Route("GetAddress")]
        public IActionResult GetAddress()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(_addressServiceClient.GetAddressForProfile(userId));
        }
    }
}
