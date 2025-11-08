using GameOnline.Common.Core;
using GameOnline.Core.Services.PropertyService.Queries.PropertyGroup;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.PropertyService.Commands.PropertyGroup;

public interface IPropertyGroupCommand
{
    OperationResult<int> CreatePropertyGroup(CreatePropertyGroupsViewmodel createPropertyGroup);
    OperationResult<int> EditPropertyGroup(EditPropertyGroupsViewmodel editPropertyGroup);
    OperationResult<int> RemovePropertyGroup(int propertyGroupId);
}

public class PropertyGroupCommand : IPropertyGroupCommand
{
    private readonly GameOnlineContext _context;
    private readonly IPropertyGroupQuery _groupQuery;

    public PropertyGroupCommand(GameOnlineContext context, IPropertyGroupQuery groupQuery)
    {
        _context = context;
        _groupQuery = groupQuery;
    }

    public OperationResult<int> CreatePropertyGroup(CreatePropertyGroupsViewmodel createPropertyGroup)
    {
        if (_groupQuery.IsPropertyGroupExist(createPropertyGroup.GroupTitle, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        DataBase.Entities.Properties.PropertyGroup propertyGroup = new DataBase.Entities.Properties.PropertyGroup()
        {
            Title = createPropertyGroup.GroupTitle,
            CreationDate = DateTime.Now
        };
        _context.PropertyGroups.Add(propertyGroup);
        _context.SaveChanges();
        return OperationResult<int>.Success(propertyGroup.Id);
    }

    public OperationResult<int> EditPropertyGroup(EditPropertyGroupsViewmodel editPropertyGroup)
    {
        var propertyGroup = _context.PropertyGroups.FirstOrDefault(x => x.Id == editPropertyGroup.PropertyGroupId);
        if (propertyGroup == null)
            return OperationResult<int>.NotFound();

        if (_groupQuery.IsPropertyGroupExist(editPropertyGroup.GroupTitle, editPropertyGroup.PropertyGroupId))
        {
            return OperationResult<int>.Duplicate();
        }

        propertyGroup.Title = editPropertyGroup.GroupTitle;
        propertyGroup.LastModified = DateTime.Now;

        _context.PropertyGroups.Update(propertyGroup);
        _context.SaveChanges();
        return OperationResult<int>.Success(propertyGroup.Id);

    }

    public OperationResult<int> RemovePropertyGroup(int propertyGroupId)
    {
        var result = _context.PropertyGroups.FirstOrDefault(x => x.Id == propertyGroupId);
        if (result == null)
        {
            return OperationResult<int>.NotFound("گروه مورد نظر یافت نشد.");
        }

        _context.PropertyGroups.Remove(result);
        _context.SaveChanges();
        return OperationResult<int>.Success(propertyGroupId);
    }
}