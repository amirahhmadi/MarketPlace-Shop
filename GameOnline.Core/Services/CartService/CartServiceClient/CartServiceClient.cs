using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using GameOnline.Core.ViewModels.UserViewmodel.Client;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;


namespace GameOnline.Core.Services.CartService.CartServiceClient;

public class CartServiceClient : ICartServiceClient
{
    private readonly GameOnlineContext _context;
    public CartServiceClient(GameOnlineContext context)
    {
        _context = context;
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

    public async Task RemoveDetailAsync(int detailId)
    {
        var findDetail = await _context.CartDetails
            .FirstOrDefaultAsync(x => x.Id == detailId);

        if (findDetail == null) return;

        _context.CartDetails.Remove(findDetail);
        await _context.SaveChangesAsync();
    }

    public void UpdateChangeCart(int detailId, int count)
    {
        var findDetail = _context.CartDetails
            .FirstOrDefault(x => x.Id.Equals(detailId));

        findDetail.Count = count;
        _context.Update(findDetail);
        _context.SaveChanges();
    }

    public OperationResult<int> UpdateCheckCart(List<GetCartDetailsViewmodel> details)
    {
        var productPriceId = details.Select(x => x.CartDetailId).ToList();

        var findProductPrice = _context.CartDetails
            .Where(x => productPriceId.Contains(x.Id))
            .AsNoTracking().ToList();

        foreach (var item in findProductPrice)
        {
            var detail = details.Where(x => x.CartDetailId == item.Id).FirstOrDefault();
            if (detail.IsRemove)
            {
                _context.CartDetails.Remove(item);
            }
            else
            {
                item.Price = detail.NewPrice.Value;
                _context.CartDetails.Update(item);
            }

        }
        _context.SaveChanges();
        return OperationResult<int>.Success(1);
    }
}