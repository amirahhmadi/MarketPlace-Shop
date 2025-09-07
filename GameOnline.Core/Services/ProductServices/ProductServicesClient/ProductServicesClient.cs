using System.Security.Cryptography.X509Certificates;
using GameOnline.Core.ViewModels.ProductViewmodel.Client;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.ProductServices.ProductServicesClient;

public class ProductServicesClient : IProductServicesClient
{
    private readonly GameOnlineContext _context;
    public ProductServicesClient(GameOnlineContext context)
    {
        _context = context;
    }
    public GetDetailProductClientViewmodel? GetDetailProductById(int productId)
    {
        return (from p in _context.Products
                join c in _context.Categories on p.CategoryId equals c.Id
                join b in _context.Brands on p.BrandId equals b.Id
                where p.Id == productId
                select new GetDetailProductClientViewmodel
                {
                    ProductId = p.Id,
                    FaTitle = p.FaTitle,
                    EnTitle = p.EnTitle,
                    ImageName = p.ImageName,
                    BrandName = b.FaTitle,
                    CategoryFa = c.FaTitle,
                    Score = p.Score
                }).AsNoTracking().SingleOrDefault();
    }

    public List<GetProductForCategoryViewmodel> GetProductForCategory(int categoryId)
    {
        return (from p in _context.Products
                join Pprice in _context.ProductPrices on p.Id equals Pprice.ProductId
                join c in _context.Categories on p.CategoryId equals c.Id
                where p.CategoryId == categoryId
                select new GetProductForCategoryViewmodel
                {
                    FaTitle = p.FaTitle,
                    ImageName = p.ImageName,
                    ProductId = p.Id,
                    CategoryName = c.FaTitle,
                    GetProductPrices = new GetProductPriceForProductViewmodel()
                    {
                        MainPrice = Pprice.Price,
                        SpecialPrice = Pprice.SpecialPrice,
                        EndDate = Pprice.EndDisCount,
                        StartDate = Pprice.StartDisCount
                    }
                }).AsNoTracking().ToList();
    }

    public List<GetProductGalleriesViewmodel> GetProductGalleries(int productId)
    {
        return _context.ProductGalleries
            .Where(x => x.ProductId == productId)
            .Select(x => new GetProductGalleriesViewmodel()
            {
                ImageName = x.ImageName
            }).AsNoTracking().ToList();
    }

    public List<GetProductPriceClientViewmodel> GetProductPriceClient(int productId)
    {
        var productPrice = (from pPrice in _context.ProductPrices
                            join g in _context.Guarantees on pPrice.GuaranteeId equals g.Id
                            join c in _context.Colors on pPrice.ColorId equals c.Id
                            where (pPrice.ProductId.Equals(productId) && pPrice.Count > 0)
                            select new GetProductPriceClientViewmodel
                            {
                                ProductPriceId = pPrice.Id,
                                ColorCode = c.Code,
                                ColorId = c.Id,
                                Count = pPrice.Count,
                                MaxOrderCount = pPrice.MaxOrderCount,
                                Price = pPrice.Price, // 👈 این همون قیمت اصلیه
                                MainPrice = pPrice.Price, // 👈 اضافه شد (قیمت مرجع)
                                SpecialPrice = pPrice.SpecialPrice,
                                StartDisCount = pPrice.StartDisCount,
                                EndDisCount = pPrice.EndDisCount,
                                SubmitDate = pPrice.SubmitDate,
                                SellerId = pPrice.SellerId,
                                GuaranteeName = g.GuaranteeName,
                                GuaranteeId = pPrice.GuaranteeId,
                            }).AsNoTracking().ToList();

        return productPrice;
    }


    public List<GetPropertyForProductClientViewmodel> GetPropertyForProductClient(int productId)
    {
        return (from pProperty in _context.PropertyProducts
                join v in _context.PropertyValues on pProperty.PropertyValueId equals v.Id
                join n in _context.PropertyNames on v.PropertyNameId equals n.Id
                join g in _context.PropertyGroups on n.GroupId equals g.Id

                where (pProperty.ProductId == productId)
                select new GetPropertyForProductClientViewmodel
                {
                    GroupTitle = g.Title,
                    NameTitle = n.Title,
                    Value = v.Value,
                }).AsNoTracking().ToList();
    }

    public GetReviewForClientViewmodel? GetReviewForClient(int productId)
    {
        return _context.ProductReviews
            .Where(x => x.ProductId == productId)
            .Select(x => new GetReviewForClientViewmodel()
            {
                Negative = x.Negative,
                Positive = x.Positive,
                Review = x.Review
            }).AsNoTracking().FirstOrDefault();
    }

    public List<GetSellerClientViewmodel> GetSellerForProductById(List<int> sellerId)
    {
        return _context.Sellers
            .Where(x => sellerId.Contains(x.Id))
            .Select(x => new GetSellerClientViewmodel()
            {
                imageName = x.imageName,
                SellerId = x.Id,
                SellerName = x.SellerNmae
            }).AsNoTracking().ToList();

    }

    public List<GetProductForCategoryViewmodel> GetDiscountedProducts()
    {
        return (from p in _context.Products
                join Pprice in _context.ProductPrices on p.Id equals Pprice.ProductId
                join c in _context.Categories on p.CategoryId equals c.Id
                where Pprice.SpecialPrice != null
                      && Pprice.SpecialPrice < Pprice.Price
                      && (Pprice.EndDisCount == null || Pprice.EndDisCount > DateTime.Now) // هنوز تخفیف تموم نشده
                      && (Pprice.StartDisCount == null || Pprice.StartDisCount <= DateTime.Now) // تخفیف شروع شده
                select new GetProductForCategoryViewmodel
                {
                    FaTitle = p.FaTitle,
                    ImageName = p.ImageName,
                    ProductId = p.Id,
                    CategoryName = c.FaTitle,
                    GetProductPrices = new GetProductPriceForProductViewmodel()
                    {
                        MainPrice = Pprice.Price,
                        SpecialPrice = Pprice.SpecialPrice,
                        EndDate = Pprice.EndDisCount,
                        StartDate = Pprice.StartDisCount
                    }
                }).AsNoTracking().ToList();
    }
}