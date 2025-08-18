using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Products;

namespace GameOnline.Core.Services.CartService.CartServiceAdmin;

public class CartServiceAdmin : ICartServiceAdmin
{
    private readonly GameOnlineContext _context;

    public CartServiceAdmin(GameOnlineContext context)
    {
        _context = context;
    }
    public OperationResult<int> AddCart(AddCartViewmodel addCart)
    {
        var findCart = FindCartIdByUserId(addCart.UserId);
        int cartId = 0;
        CartDetail? findCartDetail = null;
        if (findCart == null)
        {
            Cart cart = new Cart()
            {
                UserId = addCart.UserId,
                CreationDate = DateTime.Now,
                OrderType = OrderType.Product_selection,
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            cartId = cart.Id;
        }
        else
        {
            findCart.LastModified = DateTime.Now;
            _context.Carts.Update(findCart);
            findCartDetail = FindCartDetail(findCart.Id, addCart.ProductPriceId);
        }
        var findProductPrice = FindProductPriceById(addCart.ProductPriceId);

        if (findCartDetail == null)
        {
            findCartDetail = findCartDetail ?? new CartDetail();
            if (findProductPrice != null)
            {
                findCartDetail.Price = findProductPrice.Price;
                findCartDetail.ProductPriceId = findProductPrice.Id;
                findCartDetail.CartId = findCart == null ? cartId : findCart.Id;
                findCartDetail.Count = 1;
                findCartDetail.CreationDate = DateTime.Now;
                _context.CartDetails.Add(findCartDetail);
            }
        }
        else
        {
            if (findCartDetail != null &&
                findProductPrice.Count > findCartDetail.Count &&
                findProductPrice.MaxOrderCount > findCartDetail.Count)
            {
                findCartDetail.Count += 1;
                findCartDetail.LastModified = DateTime.Now;
                findCartDetail.Price = findProductPrice.Price;
                _context.CartDetails.Update(findCartDetail);
            }
        }

        _context.SaveChanges();

        return OperationResult<int>.Success(addCart.UserId);
    }

    public Cart? FindCartIdByUserId(int userId)
    {
        return _context.Carts
            .Where(x => x.UserId.Equals(userId))
            .FirstOrDefault(x => x.OrderType == OrderType.Product_selection);
    }

    public CartDetail? FindCartDetail(int cartId, int productPriceId)
    {
        return _context.CartDetails
            .Where(x => x.CartId.Equals(cartId))
            .FirstOrDefault(x => x.ProductPriceId.Equals(productPriceId));
    }

    public ProductPrice? FindProductPriceById(int productPriceId)
    {
        return _context.ProductPrices
            .FirstOrDefault(x => x.Id == productPriceId);
    }
}