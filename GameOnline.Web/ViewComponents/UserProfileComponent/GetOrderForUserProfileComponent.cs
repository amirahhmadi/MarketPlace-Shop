using GameOnline.Core.Services.CartService.CartServiceClient;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameOnline.Web.ViewComponents.UserProfileComponent;

public class GetOrderForUserProfileComponent : ViewComponent
{
    private readonly ICartServiceClient _cartServiceClient;

    public GetOrderForUserProfileComponent(ICartServiceClient cartServiceClient)
    {
        _cartServiceClient = cartServiceClient;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = User as ClaimsPrincipal;
        int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        return await Task.FromResult(View("GetOrderForUserProfile", _cartServiceClient.GetOrdersForProfileByUserId(userId)));
    }
}