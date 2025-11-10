using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GameOnline.Core.Services.CartService.Queries;

namespace GameOnline.Web.ViewComponents
{
    public class MiniCartViewComponent : ViewComponent
    {
        private readonly ICartServiceQuery _cartServiceQuery;

        public MiniCartViewComponent(ICartServiceQuery cartServiceQuery)
        {
            _cartServiceQuery = cartServiceQuery;
        }

        public IViewComponentResult Invoke()
        {
            if (!User.Identity.IsAuthenticated)
                return View("MiniCart", new List<GameOnline.Core.ViewModels.CartViewmodel.Client.GetCartDetailsViewmodel>());

            int userId = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = _cartServiceQuery.GetCartDetails(userId);

            return View("MiniCart", cart);
        }
    }
}