using GameOnline.Core.ViewModels.GuaranteeViewModels;

namespace GameOnline.Core.Services.GuaranteeServices.Queries;

public interface IGuaranteeServiceQuery
{
    List<GetGuaranteesViewModel> GetGuarantees();
    EditGuaranteesViewModel? GetGuaranteeById(int guaranteeId);
    bool IsGuaranteeExist(string guaranteeName, int excludeId);
}