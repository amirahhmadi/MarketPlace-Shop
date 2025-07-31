using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using GameOnline.DataBase.Entities.Properties;

namespace GameOnline.Core.Services.PropertyService.PropertyGroupService;

public interface IPropertyGroupServiceAdmin
{
    List<GetPropertyGroupsViewmodel> GetPropertyGroups();
    OperationResult<int> CreatePropertyGroup(CreatePropertyGroupsViewmodel createPropertyGroup);
    OperationResult<int> EditPropertyGroup(EditPropertyGroupsViewmodel editPropertyGroup);
    EditPropertyGroupsViewmodel? GetPropertyGroupById(int propertyGroupId);
    OperationResult<int> RemovePropertyGroup(int propertyGroupId);
}