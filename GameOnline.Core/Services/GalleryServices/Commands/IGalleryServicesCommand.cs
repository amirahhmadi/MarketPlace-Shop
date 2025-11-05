using GameOnline.Common.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.GalleryServices.Commands;

public interface IGalleryServicesCommand
{
    OperationResult<int> CreateImageForGallery(int productId, IFormFile imageName);
    public OperationResult<int> RemoveImageFromGallery(int galleryId);
}