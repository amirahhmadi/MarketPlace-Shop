using GameOnline.Core.ViewModels.GalleryViewmodel;

namespace GameOnline.Core.Services.GalleryServices.Queries;

public interface IGalleryServicesQuery
{
    List<GetImageGalleryForProductViewmodel> GetImageGalleryForProductById(int productId);
}