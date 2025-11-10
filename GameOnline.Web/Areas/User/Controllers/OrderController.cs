using System.Security.Claims;
using GameOnline.Core.Services.CartService.Queries;
using GameOnline.DataBase.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.Areas.User.Controllers
{
    public class OrderController : BaseUserController
    {
        private readonly ICartServiceQuery _cartServiceQuery;

        public OrderController(ICartServiceQuery cartServiceQuery)
        {
            _cartServiceQuery = cartServiceQuery;
        }

        [HttpGet]
        [Route("OrderDetail/{cartId}")]
        public IActionResult OrderDetail(int cartId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orderDetail = _cartServiceQuery.GetOrderDetailForProfileByCartId(userId, cartId);
            return View(orderDetail);
        }

        [Route("Orders")]
        public IActionResult Orders()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(_cartServiceQuery.GetOrdersForProfileByUserId(userId));
        }
    }
}
