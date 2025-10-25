using GameOnline.Core.ViewModels.ColorViewModels;

namespace GameOnline.Core.Services.ColorServices.Queries;

public interface IColorServicesQuery
{
    List<GetColorsViewModel> GetColors();
    EditColorsViewModel? GetColorById(int colorId);
    bool IsColorExist(string colorCode, string colorName, int excludeId);
}