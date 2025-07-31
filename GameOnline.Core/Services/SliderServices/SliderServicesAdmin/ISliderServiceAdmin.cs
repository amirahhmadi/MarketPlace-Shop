using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.SliderViewModels;

namespace GameOnline.Core.Services.SliderServices.SliderServicesAdmin;

public interface ISliderServiceAdmin
{
    List<GetSlidersViewModel> GetSliders();
    OperationResult<int> CreateSlider(CreateSlidersViewModel createSlider);
    EditSlidersViewModel? GetSliderById(int sliderId);
    OperationResult<int> EditSlider(EditSlidersViewModel editSlider);
    OperationResult<int> RemoveSlider(RemoveSlidersViewModel removeSlider);

}