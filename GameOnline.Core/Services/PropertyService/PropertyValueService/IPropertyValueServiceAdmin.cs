using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;

namespace GameOnline.Core.Services.PropertyService.PropertyValueService
{
    public interface IPropertyValueServiceAdmin
    {
        List<GetPropertyValuesViewmodel> GetPropertyValues();
        OperationResult<int> CreatePropertyValue(CreatePropertyValueViewmodel propertyValue);
        OperationResult<int> EditPropertyValue(EditPropertyValueViewmodel editPropertyValue);
        EditPropertyValueViewmodel? GetPropertyValueById(int propertyValueId);
        OperationResult<int> RemovePropertyValue(int propertyValueId);
    }
}
