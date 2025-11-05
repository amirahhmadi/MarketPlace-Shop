using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.DiscountViewModels;

namespace GameOnline.Core.Services.DiscountServices.Commands;

public interface IDiscountServicesCommand
{
    OperationResult<int> CreateDiscount(CreateDiscountViewModels createDiscountViewModels);
}