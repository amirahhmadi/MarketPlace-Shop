using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.GuaranteeViewModels;

namespace GameOnline.Core.Services.GuaranteeServices.Commands;

public interface IGuaranteeServiceCommand
{
    OperationResult<int> CreateGuarantee(CreateGuaranteesViewModel createGuarantee);
    OperationResult<int> EditGuarantee(EditGuaranteesViewModel editGuarantee);
    OperationResult<int> RemoveGuarantee(RemoveGuaranteesViewModel removeGuarantee);
}