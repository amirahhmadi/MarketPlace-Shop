using GameOnline.Core.Services.CartService.CartServiceClient;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameOnline.Web.ViewComponents;

public class MiniCartMobileViewComponent : ViewComponent
{
    private readonly ICartServiceClient _cartServiceClient;

    public MiniCartMobileViewComponent(ICartServiceClient cartServiceClient)
    {
        _cartServiceClient = cartServiceClient;
    }

    public IViewComponentResult Invoke()
    {
        if (!User.Identity.IsAuthenticated)
            return View("MiniCartMobile", new List<GameOnline.Core.ViewModels.CartViewmodel.Client.GetCartDetailsViewmodel>());

        int userId = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
        var cart = _cartServiceClient.GetCartDetails(userId);

        return View("MiniCartMobile", cart);
    }
}