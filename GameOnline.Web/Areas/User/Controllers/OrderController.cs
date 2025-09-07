using System.Security.Claims;
using GameOnline.Core.Services.CartService.CartServiceClient;
using GameOnline.DataBase.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.User.Controllers
{
    public class OrderController : BaseUserController
    {
        private readonly ICartServiceClient _cartServiceClient;

        public OrderController(ICartServiceClient cartServiceClient)
        {
            _cartServiceClient = cartServiceClient;
        }

        [HttpGet]
        [Route("OrderDetail/{cartId}")]
        public IActionResult OrderDetail(int cartId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orderDetail = _cartServiceClient.GetOrderDetailForProfileByCartId(userId, cartId);
            return View(orderDetail);
        }

        [Route("Orders")]
        public IActionResult Orders()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(_cartServiceClient.GetOrdersForProfileByUserId(userId));
        }
    }
}
