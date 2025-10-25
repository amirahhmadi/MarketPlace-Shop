using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.BrandServices.Queries;

public class BrandServiceQuery : IBrandServiceQuery
{
    private readonly GameOnlineContext _context;
    public BrandServiceQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public EditBrandsViewModel GetBrandById(int brandId)
    {
        return _context.Brands
            .Where(x => x.Id == brandId)
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

    public bool IsBrandExist(string faTitle, string enTitle, int excludeId)
    {
        return _context.Brands.Any(x =>
            (x.FaTitle == faTitle.Trim() || x.EnTitle == enTitle.Trim()) &&
            x.Id != excludeId);
    }
}