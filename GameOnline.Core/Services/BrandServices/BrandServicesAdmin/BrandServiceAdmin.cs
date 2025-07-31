using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Brands;
using Microsoft.EntityFrameworkCore;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Common.Core;

namespace GameOnline.Core.Services.BrandServices.BrandServicesAdmin;

public class BrandServiceAdmin : IBrandServiceAdmin
{
    private readonly GameOnlineContext _context;

    public BrandServiceAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreateBrand(CreateBrandsViewModel createBrand)
    {
        if (IsBrandExist(createBrand.FaTitle,createBrand.EnTitle,0))
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

        if (IsBrandExist(editBrand.FaTitle, editBrand.EnTitle, editBrand.BrandId))
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

    public EditBrandsViewModel GetBrandById(int BrandId)
    {
        return _context.Brands
            .Where(x => x.Id == BrandId)
            .Select(x => new EditBrandsViewModel()
            {
                BrandId = x.Id,
                EnTitle = x.EnTitle,
                FaTitle = x.FaTitle,
                Description = x.Description,
                OldImgName = x.ImageName
            }).SingleOrDefault();
    }

    public List<GetBrandsViewModel> GetBrands()
    {
        return _context.Brands
        .Select(x => new GetBrandsViewModel()
        {
            BrandId = x.Id,
            EnTitle = x.EnTitle,
            FaTitle = x.FaTitle,
            ImageName = x.ImageName
        }).AsNoTracking().ToList();

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

    public bool IsBrandExist(string faTitle,string enTitle, int excludeId)
    {
        return _context.Brands.Any(x =>
            (x.FaTitle == faTitle.Trim() || x.EnTitle == enTitle.Trim()) &&
            x.Id != excludeId);
    }
}