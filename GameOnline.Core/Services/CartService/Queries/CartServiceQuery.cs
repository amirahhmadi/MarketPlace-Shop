using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.CartViewmodel.Admin;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using GameOnline.Core.ViewModels.UserViewmodel.Client;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace GameOnline.Core.Services.CartService.Queries;

public class CartServiceQuery : ICartServiceQuery
{
    private readonly GameOnlineContext _context;
    public CartServiceQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> FindCartIdByUserId(int userId)
    {
        var findcart = _context.Carts
            .Where(c => c.UserId == userId)
            .FirstOrDefault(c => c.OrderType == OrderType.Product_selection);

        return OperationResult<int>.Success(findcart.Id);
    }

    public Cart? FindCartByUserId(int userId)
    {
        return _context.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefault(c => c.UserId == userId && c.OrderType == OrderType.Product_selection);
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

    public Cart? FindCartById(int cartId)
    {
        return _context.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefault(c => c.Id == cartId && c.OrderType == OrderType.Product_selection);
    }

    public List<GetCartDetailsViewmodel> GetCartDetails(int userId)
    {
        var q = (from c in _context.Carts
                join cd in _context.CartDetails on c.Id equals cd.CartId
                join pPrice in _context.ProductPrices on cd.ProductPriceId equals pPrice.Id
                join product in _context.Products on pPrice.ProductId equals product.Id
                join g in _context.Guarantees on pPrice.GuaranteeId equals g.Id
                join color in _context.Colors on pPrice.ColorId equals color.Id
                join seller in _context.Sellers on pPrice.SellerId equals seller.Id

                where (c.UserId == userId && c.OrderType == OrderType.Product_selection)

                select new GetCartDetailsViewmodel
                {
                    SpecialPrice = pPrice.SpecialPrice,
                    MainPrice = pPrice.Price,
                    Price = cd.Price,
                    CartCount = cd.Count,
                    ProductCount = pPrice.Count,
                    ProductMaxUserCount = pPrice.MaxOrderCount,
                    CartId = c.Id,
                    CartDetailId = cd.Id,
                    ColorCode = color.Code,
                    ColorName = color.ColorName,
                    GuaranteeName = g.GuaranteeName,
                    ProductEnTitle = product.EnTitle,
                    ProductFaTitle = product.FaTitle,
                    ProductId = product.Id,
                    ProductPriceId = pPrice.Id,
                    ProductImg = product.ImageName,
                    SellerName = seller.SellerNmae,
                    StartDisCount = pPrice.StartDisCount,
                    EndDisCount = pPrice.EndDisCount,
                    LastModifiedDate = c.LastModified,
                    Score = product.Score
                })
            .AsNoTracking()
            .ToList();

        return q;
    }

    public List<GetOrderDetailViewmodel> GetOrderDetailForProfileByCartId(int userId, int cartId)
    {
        return (from c in _context.Carts
                join cd in _context.CartDetails on c.Id equals cd.CartId
                join Pprice in _context.ProductPrices on cd.ProductPriceId equals Pprice.Id
                join p in _context.Products on Pprice.ProductId equals p.Id

                where (c.UserId == userId && c.Id.Equals(cartId) && c.OrderType != OrderType.Product_selection)
                select new GetOrderDetailViewmodel
                {
                    CartId = c.Id,
                    CreationDate = c.CreationDate,
                    LastModifiedDate = c.LastModified,
                    OrderType = c.OrderType,
                    Price = c.SumOrder,

                    OrderDetail = new OrderDeatilViewmodel
                    {
                        Count = cd.Count,

                        // قیمت اصلی (مثلا از جدول ProductPrices)
                        OriginalPrice = Pprice.Price,

                        // قیمت بعد از تخفیف (قیمت نهایی در CartDetails ذخیره شده)
                        Price = cd.Price,

                        FaTitle = p.FaTitle,
                        ImageName = p.ImageName,
                        ProductId = p.Id
                    }
                }).AsNoTracking()
            .ToList();
    }

    public List<GetOrdersViewmodel> GetOrdersForProfileByUserId(int userId)
    {
        return (from c in _context.Carts
                join cd in _context.CartDetails on c.Id equals cd.CartId

                where (c.UserId.Equals(userId))
                select new GetOrdersViewmodel
                {
                    CartId = c.Id,
                    CreationDate = c.CreationDate,
                    LastModified = c.LastModified,
                    OrderType = c.OrderType,
                    Price = c.SumOrder,
                    sumOrder = new SumOrderProfileViewmodel
                    {
                        Price = cd.Price,
                        Count = cd.Count,
                        CreationDate = cd.CreationDate,
                        LastModified = cd.LastModified,
                    }
                }).AsNoTracking()
            .ToList();
    }

    public List<GetOrdersViewmodel> GetOrdersForUserId(int userId)
    {
        return (from c in _context.Carts
            join cd in _context.CartDetails on c.Id equals cd.CartId
            where (c.UserId.Equals(userId))
            select new GetOrdersViewmodel()
            {
                CreationDate = c.CreationDate,
                CartId = c.Id,
                LastModified = c.LastModified,
                OrderType = c.OrderType,
                Price = c.SumOrder,
                sumOrder = new SumOrderProfileViewmodel()
                {
                    Price = cd.Price,
                    Count = cd.Count,
                    CreationDate = cd.CreationDate,
                    LastModified = cd.LastModified,
                }
            }).AsNoTracking().ToList();
    }
}