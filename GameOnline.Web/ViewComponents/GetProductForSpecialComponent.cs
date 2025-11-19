using GameOnline.Core.Services.ProductServices.Queries;
using GameOnline.Core.Services.SliderServices.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GameOnline.Web.ViewComponents;

public class GetProductForSpecialComponent : ViewComponent
{
    private readonly ISliderServiceQuery _sliderServiceQuery;

    public GetProductForSpecialComponent(ISliderServiceQuery sliderServiceQuery)
    {
        _sliderServiceQuery = sliderServiceQuery;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult(View("GetSpecialSlider", _sliderServiceQuery.SpecialSlider()));
    }
}