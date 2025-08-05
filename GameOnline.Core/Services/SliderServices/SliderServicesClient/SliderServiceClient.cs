using System.Security.Cryptography.X509Certificates;
using GameOnline.Core.ViewModels.SliderViewModels.Client;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.SliderServices.SliderServicesClient;

public class SliderServiceClient : ISliderServiceClient
{
    private readonly GameOnlineContext _context;

    public SliderServiceClient(GameOnlineContext context)
    {
        _context = context;
    }

    public List<GetSlidersViewModel> GetSliders()
    {
        return _context.Sliders.Where(x => x.IsActive)
            .Select(x => new GetSlidersViewModel()
            {
                SliderId = x.Id,
                Link = x.Link,
                SliderSort = x.Sort,
                SliderImage = x.ImageName
            }).AsNoTracking()
            .OrderBy(x=>x.SliderSort)
            .ToList();
    }
}