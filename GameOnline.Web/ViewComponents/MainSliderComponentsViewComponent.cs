using GameOnline.Core.Services.SliderServices.Queries;
using Microsoft.AspNetCore.Mvc;
using GameOnline.Core.ViewModels.SliderViewModels.Client;
using System.Linq;
using System.Threading.Tasks;

namespace GameOnline.Web.ViewComponents
{
    public class MainSliderComponentsViewComponent : ViewComponent
    {
        private readonly ISliderServiceQuery _sliderServiceQuery;

        public MainSliderComponentsViewComponent(ISliderServiceQuery sliderServiceQuery)
        {
            _sliderServiceQuery = sliderServiceQuery;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // دریافت لیست ادمین
            var adminSliders = _sliderServiceQuery.GetSliders();

            // تبدیل مدل Admin به مدل Client
            var clientSliders = adminSliders.Select(s => new GetSlidersViewModel
            {
                SliderId = s.SliderId,
                SliderImage = s.ImageName, // یا هر نامی که در Admin مدلش هست
                Link = s.Link,             // در صورت وجود
            }).ToList();

            return await Task.FromResult(View("MainSlider", clientSliders));
        }
    }
}