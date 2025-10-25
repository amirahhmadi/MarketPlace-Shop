using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using GameOnline.Core.ViewModels.UserViewmodel.Client;
using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Products;

namespace GameOnline.Core.Services.CartService.Queries;

public interface ICartServiceQuery
{
    List<GetCartDetailsViewmodel> GetCartDetails(int userId);
    List<GetOrdersViewmodel> GetOrdersForProfileByUserId(int userId);
    List<GetOrdersViewmodel> GetOrdersForUserId(int userId);
    List<GetOrderDetailViewmodel> GetOrderDetailForProfileByCartId(int userId, int cartId);
    OperationResult<int> FindCartIdByUserId(int userId);
    Cart? FindCartById(int cartId);
    Cart? FindCartByUserId(int userId);
    ProductPrice? FindProductPriceById(int productPriceId);
    CartDetail? FindCartDetail(int cartId, int productPriceId);
}