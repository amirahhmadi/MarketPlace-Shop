using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.DataBase.Entities.Carts;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.CartService.CartServiceAdmin;

public interface ICartServiceAdmin
{
    OperationResult<int> AddCart(AddCartViewmodel addCart);
    OperationResult<int> FindCartIdByUserId(int userId);
    Task<OperationResult<string>> Payment(int cartId);
    Task<OperationResult<int>> VerificationZarinPal(int cartId, string authority);
}