using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using GameOnline.Core.ViewModels.UserViewmodel.Client;

namespace GameOnline.Core.Services.CartService.CartServiceClient;

public interface ICartServiceClient
{
    List<GetCartDetailsViewmodel> GetCartDetails(int userId);
    OperationResult<int> UpdateCheckCart(List<GetCartDetailsViewmodel> details);
    void UpdateChangeCart(int detailId, int count);
    public Task RemoveDetailAsync(int detailId);
    List<GetOrdersViewmodel> GetOrdersForProfileByUserId(int userId);
    List<GetOrderDetailViewmodel> GetOrderDetailForProfileByCartId(int userId, int cartId);
}