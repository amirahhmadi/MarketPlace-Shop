using GameOnline.Common.Core;
using GameOnline.Core.Services.SliderServices.SliderServicesClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.ViewComponents;

public class MainSliderComponentsViewComponent : ViewComponent
{
    private readonly ISliderServiceClient _sliderServiceClient;
    public MainSliderComponentsViewComponent(ISliderServiceClient sliderServiceClient)
    {
        _sliderServiceClient = sliderServiceClient;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult(View("MainSlider",_sliderServiceClient.GetSliders()));
    }
}