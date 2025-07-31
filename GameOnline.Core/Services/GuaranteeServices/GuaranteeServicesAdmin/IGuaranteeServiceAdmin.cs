using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.GuaranteeViewModels;

namespace GameOnline.Core.Services.GuaranteeServices.GuaranteeServicesAdmin;

public interface IGuaranteeServiceAdmin
{
    List<GetGuaranteesViewModel> GetGuarantees();
    OperationResult<int> CreateGuarantee(CreateGuaranteesViewModel createGuarantee);
    EditGuaranteesViewModel? GetGuaranteeById(int guaranteeId);
    OperationResult<int> EditGuarantee(EditGuaranteesViewModel editGuarantee);
    OperationResult<int> RemoveGuarantee(RemoveGuaranteesViewModel removeGuarantee);
}