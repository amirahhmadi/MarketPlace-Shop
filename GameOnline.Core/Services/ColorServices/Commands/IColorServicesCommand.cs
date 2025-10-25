using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Guarantees;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.ColorServices.Commands;

public interface IColorServicesCommand
{
    OperationResult<int> CreateColor(CreateColorsViewModel createColors);
    OperationResult<int> EditColor(EditColorsViewModel editColors);
    OperationResult<int> RemoveColor(RemoveColorsViewModel removeColors);
}