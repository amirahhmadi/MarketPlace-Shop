using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.Core.ViewModels.GuaranteeViewModels;

namespace GameOnline.Core.Services.ColorServices.ColorServicesAdmin;

public interface IColorServicesAdmin
{
    List<GetColorsViewModel> GetColors();
    OperationResult<int> CreateColor(CreateColorsViewModel createColors);
    EditColorsViewModel? GetColorById(int colorId);
    OperationResult<int> EditColor(EditColorsViewModel editColors);
    OperationResult<int> RemoveColor(RemoveColorsViewModel removeColors);
}