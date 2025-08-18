using GameOnline.Core.Services.CartService.CartServiceClient;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameOnline.Web.ViewComponents
{
    public class MiniCartViewComponent : ViewComponent
    {
        private readonly ICartServiceClient _cartServiceClient;

        public MiniCartViewComponent(ICartServiceClient cartServiceClient)
        {
            _cartServiceClient = cartServiceClient;
        }

        public IViewComponentResult Invoke()
        {
            if (!User.Identity.IsAuthenticated)
                return View("MiniCart", new List<GameOnline.Core.ViewModels.CartViewmodel.Client.GetCartDetailsViewmodel>());

            int userId = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = _cartServiceClient.GetCartDetails(userId);

            return View("MiniCart", cart);
        }
    }
}