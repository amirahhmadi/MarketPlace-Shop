using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.BrandServices.Queries;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Brands;

namespace GameOnline.Core.Services.BrandServices.Commands;

public class BrandServiceCommand : IBrandServiceCommand
{
    private readonly GameOnlineContext _context;
    private readonly IBrandServiceQuery _brandQuery;
    public BrandServiceCommand(GameOnlineContext context, IBrandServiceQuery brandQuery)
    {
        _context = context;
        _brandQuery = brandQuery;
    }
    public OperationResult<int> CreateBrand(CreateBrandsViewModel createBrand)
    {
        if (_brandQuery.IsBrandExist(createBrand.FaTitle, createBrand.EnTitle, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        string imageName = createBrand.ImageName.UploadImage(PathTools.PathBrandImageAdmin);

        Brand brand = new Brand()
        {
            CreationDate = DateTime.Now,
            EnTitle = createBrand.EnTitle,
            FaTitle = createBrand.FaTitle,
            Description = createBrand.Description,
            ImageName = imageName

        };
        _context.Brands.Add(brand);
        _context.SaveChanges();
        return OperationResult<int>.Success(brand.Id);
    }

    public OperationResult<int> EditBrand(EditBrandsViewModel editBrand)
    {
        var brand = _context.Brands
            .FirstOrDefault(x => x.Id == editBrand.BrandId && x.IsRemove == false);

        if (brand == null)
            return OperationResult<int>.NotFound();

        if (_brandQuery.IsBrandExist(editBrand.FaTitle, editBrand.EnTitle, editBrand.BrandId))
        {
            return OperationResult<int>.Duplicate();
        }

        if (editBrand.ImageName is { Length: > 0 })
        {
            ImageExtension.RemoveImage(brand.ImageName, PathTools.PathBrandImageAdmin);
            brand.ImageName = editBrand.ImageName.UploadImage(PathTools.PathBrandImageAdmin);
        }

        brand.FaTitle = editBrand.FaTitle;
        brand.EnTitle = editBrand.EnTitle;
        brand.Description = editBrand.Description;
        brand.LastModified = DateTime.Now;

        _context.Brands.Update(brand);
        _context.SaveChanges();
        return OperationResult<int>.Success(editBrand.BrandId);

    }

    public OperationResult<int> RemoveBrand(RemoveBrandsViewModel removeBrand)
    {
        var brand = _context.Brands
            .FirstOrDefault(x => x.Id == removeBrand.BrandId);

        if (brand == null)
            return OperationResult<int>.NotFound();

        brand.IsRemove = true;
        brand.RemoveDate = DateTime.Now;

        _context.Brands.Update(brand);
        _context.SaveChanges();
        return OperationResult<int>.Success(removeBrand.BrandId);
    }
}