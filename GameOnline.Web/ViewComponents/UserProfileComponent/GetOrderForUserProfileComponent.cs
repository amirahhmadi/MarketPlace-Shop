using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GameOnline.Core.Services.CartService.Queries;

namespace GameOnline.Web.ViewComponents.UserProfileComponent;

public class GetOrderForUserProfileComponent : ViewComponent
{
    private readonly ICartServiceQuery _cartServiceQuery;

    public GetOrderForUserProfileComponent(ICartServiceQuery cartServiceQuery)
    {
        _cartServiceQuery = cartServiceQuery;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = User as ClaimsPrincipal;
        int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        return await Task.FromResult(View("GetOrderForUserProfile", _cartServiceQuery.GetOrdersForProfileByUserId(userId)));
    }
}