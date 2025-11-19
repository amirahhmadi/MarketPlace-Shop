using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.SliderViewModels.Admin;
using GameOnline.Core.ViewModels.SliderViewModels.Queries;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.SliderServices.Queries;

public class SliderServiceQuery : ISliderServiceQuery
{
    private readonly GameOnlineContext _context;

    public SliderServiceQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public EditSlidersViewModel? GetSliderById(int sliderId)
    {
        return _context.Sliders
            .Where(x => x.Id == sliderId)
            .Select(x => new EditSlidersViewModel()
            {
                SliderId = x.Id,
                OldImgName = x.ImageName,
                IsActive = x.IsActive,
                Link = x.Link,
                Sort = x.Sort
            }).AsNoTracking().SingleOrDefault();
    }

    public List<GetSlidersViewModel> GetSliders()
    {
        return _context.Sliders.Select(x => new GetSlidersViewModel()
        {
            ImageName = x.ImageName,
            IsActive = x.IsActive,
            SliderId = x.Id
        }).AsNoTracking().ToList();
    }

    public bool IsSliderExist(string link, string imageName, int excludeId)
    {
        return _context.Brands.Any(x =>
            (x.FaTitle == link.Trim() || x.EnTitle == imageName.Trim()) &&
            x.Id != excludeId);
    }

    public List<SpecialSliderViewmodel> SpecialSlider()
    {
        var now = DateTime.Now;

        return _context.ProductPrices
            .Where(pp => pp.EndDisCount > now
                         && pp.SpecialPrice != null
                         && pp.SpecialPrice > 0)
            .Join(_context.Products,
                pp => pp.ProductId,
                p => p.Id,
                (pp, p) => new SpecialSliderViewmodel
                {
                    ProductId = p.Id,
                    FaTitle = p.FaTitle,
                    ImgName = p.ImageName,
                    MinePrice = pp.Price,
                    SpecialPrice = pp.SpecialPrice.Value,
                    EndDisCount = pp.EndDisCount.SpecialDisCount()
                })
            .ToList();
    }
}