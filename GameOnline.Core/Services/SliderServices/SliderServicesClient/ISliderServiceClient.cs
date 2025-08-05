using GameOnline.Core.ViewModels.SliderViewModels.Client;

namespace GameOnline.Core.Services.SliderServices.SliderServicesClient;

public interface ISliderServiceClient
{
    List<GetSlidersViewModel> GetSliders();
}