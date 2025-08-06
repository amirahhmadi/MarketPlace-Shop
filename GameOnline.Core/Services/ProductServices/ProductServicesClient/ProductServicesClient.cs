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
                CategoryFa = c.FaTitle
            }).AsNoTracking().SingleOrDefault();
    }
}