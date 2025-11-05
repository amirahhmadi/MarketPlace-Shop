using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.SliderViewModels.Admin;

namespace GameOnline.Core.Services.SliderServices.Commands;

public interface ISliderServiceCommand
{
    OperationResult<int> CreateSlider(CreateSlidersViewModel createSlider);
    OperationResult<int> EditSlider(EditSlidersViewModel editSlider);
    OperationResult<int> RemoveSlider(RemoveSlidersViewModel removeSlider);
}