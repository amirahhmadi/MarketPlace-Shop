using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.Core.ViewModels.GalleryViewmodel;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.GalleryServices.GalleryServicesAdmin;

public class GalleryServicesAdmin : IGalleryServicesAdmin
{
    private readonly GameOnlineContext _context;

    public GalleryServicesAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreateImageForGallery(int productId, IFormFile imageName)
    {
        string imgName = imageName.UploadImage(PathTools.PathGalleryImageAdmin);

        ProductGallery productGallery = new ProductGallery()
        {
            CreationDate = DateTime.Now,
            ImageName = imgName,
            ProductId = productId
        };
        _context.ProductGalleries.Add(productGallery);
        _context.SaveChanges();
        return OperationResult<int>.Success(productGallery.Id);
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

    public OperationResult<int> RemoveImageFromGallery(int galleryId)
    {
        var result = _context.ProductGalleries.FirstOrDefault(x => x.Id == galleryId);
        if (result == null)
        {
            return OperationResult<int>.NotFound("تصویر مورد نظر یافت نشد.");
        }

        // حذف فایل تصویر از مسیر
        var filePath = Path.Combine(PathTools.PathGalleryImageAdmin, result.ImageName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        _context.ProductGalleries.Remove(result);
        _context.SaveChanges();
        return OperationResult<int>.Success(galleryId);
    }
}