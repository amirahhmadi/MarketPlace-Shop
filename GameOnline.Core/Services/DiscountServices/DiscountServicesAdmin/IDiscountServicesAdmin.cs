using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.DiscountViewModels;

namespace GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;

public interface IDiscountServicesAdmin
{
    List<GetDiscountViewModels> GetDiscount();
    OperationResult<int> CreateDiscount(CreateDiscountViewModels createDiscountViewModels);
}