using GameOnline.Core.ViewModels.SliderViewModels.Admin;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.SliderServices.Queries;

public class SliderServiceQuery :ISliderServiceQuery
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
}