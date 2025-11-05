using GameOnline.Core.ViewModels.SliderViewModels.Admin;

namespace GameOnline.Core.Services.SliderServices.Queries;

public interface ISliderServiceQuery
{
    List<GetSlidersViewModel> GetSliders();
    EditSlidersViewModel? GetSliderById(int sliderId);
    bool IsSliderExist(string link, string imageName, int excludeId);
}