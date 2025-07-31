using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.GalleryViewmodel;
using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.Services.GalleryServices.GalleryServicesAdmin;

public interface IGalleryServicesAdmin
{
    List<GetImageGalleryForProductViewmodel> GetImageGalleryForProductById(int productId);
    OperationResult<int> CreateImageForGallery(int productId, IFormFile imageName);
    public OperationResult<int> RemoveImageFromGallery(int galleryId);

}