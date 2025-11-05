using GameOnline.Core.ViewModels.DiscountViewModels;

namespace GameOnline.Core.Services.DiscountServices.Queries;

public interface IDiscountServicesQuery
{
    List<GetDiscountViewModels> GetDiscount();
    bool IsDiscountExist(string code, int excludeId);
}