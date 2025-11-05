using GameOnline.Core.ViewModels.GalleryViewmodel;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.GalleryServices.Queries;

public class GalleryServicesQuery : IGalleryServicesQuery
{
    private readonly GameOnlineContext _context;
    public GalleryServicesQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public List<GetImageGalleryForProductViewmodel> GetImageGalleryForProductById(int productId)
    {
        return _context.ProductGalleries.Where(x => x.ProductId == productId)
            .Select(x => new GetImageGalleryForProductViewmodel
            {
                GalleryId = x.Id,
                ProductId = x.ProductId,
                ImageName = x.ImageName
            }).AsNoTracking().ToList();
    }
}