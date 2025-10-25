using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.Core.ViewModels.CartViewmodel.Client;

namespace GameOnline.Core.Services.CartService.Commands;

public interface ICartServiceCommand
{
    OperationResult<int> UpdateCheckCart(List<GetCartDetailsViewmodel> details);
    void UpdateChangeCart(int detailId, int count);
    public Task RemoveDetailAsync(int detailId);

    OperationResult<int> AddCart(AddCartViewmodel addCart);
    Task<OperationResult<string>> Payment(int cartId);
    Task<OperationResult<int>> VerificationZarinPal(int cartId, string authority);
}