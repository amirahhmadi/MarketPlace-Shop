using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;

namespace GameOnline.Core.Services.PropertyService.PropertyNameService;

public interface IPropertyNameServiceAdmin
{
    List<GetPropertyNameViewmodel> GetPropertyName();
    OperationResult<int> CreatePropertyName(CreatePropertyNameViewmodel createPropertyName);
    OperationResult<int> EditPropertyName(EditPropertyNameViewmodel editPropertyName);
    EditPropertyNameViewmodel? GetPropertyNameById(int propertyNameId);
    OperationResult<int> RemovePropertyName(int propertyNameId);
}