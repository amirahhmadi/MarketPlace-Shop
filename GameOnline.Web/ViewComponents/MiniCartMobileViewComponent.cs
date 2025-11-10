using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GameOnline.Core.Services.CartService.Queries;

namespace GameOnline.Web.ViewComponents;

public class MiniCartMobileViewComponent : ViewComponent
{
    private readonly ICartServiceQuery _cartServiceQuery;

    public MiniCartMobileViewComponent(ICartServiceQuery cartServiceQuery)
    {
        _cartServiceQuery = cartServiceQuery;
    }

    public IViewComponentResult Invoke()
    {
        if (!User.Identity.IsAuthenticated)
            return View("MiniCartMobile", new List<GameOnline.Core.ViewModels.CartViewmodel.Client.GetCartDetailsViewmodel>());

        int userId = int.Parse(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
        var cart = _cartServiceQuery.GetCartDetails(userId);

        return View("MiniCartMobile", cart);
    }
}