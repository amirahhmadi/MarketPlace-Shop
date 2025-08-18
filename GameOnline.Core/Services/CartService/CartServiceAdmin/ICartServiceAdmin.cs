using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.DataBase.Entities.Carts;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.CartService.CartServiceAdmin;

public interface ICartServiceAdmin
{
    OperationResult<int> AddCart(AddCartViewmodel addCart);
    public CartDetail? FindCartDetail(int cartId, int productPriceId);
}