using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Properties;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.PropertyService.PropertyGroupService;

public class PropertyGroupServiceAdmin : IPropertyGroupServiceAdmin
{
    private readonly GameOnlineContext _context;
    public PropertyGroupServiceAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreatePropertyGroup(CreatePropertyGroupsViewmodel createPropertyGroup)
    {
        if (IsPropertyGroupExist(createPropertyGroup.GroupTitle, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        PropertyGroup propertyGroup = new PropertyGroup()
        {
            Title = createPropertyGroup.GroupTitle,
            CreationDate = DateTime.Now
        };
        _context.PropertyGroups.Add(propertyGroup);
        _context.SaveChanges();
        return OperationResult<int>.Success(propertyGroup.Id);
    }

    public List<GetPropertyGroupsViewmodel> GetPropertyGroups()
    {
        return _context.PropertyGroups.Select(x => new GetPropertyGroupsViewmodel
        {
            PropertyGroupId = x.Id,
            GroupTitle = x.Title,
        }).AsNoTracking().ToList();
    }

    public bool IsPropertyGroupExist(string groupTitle, int excludeId)
    {
        return _context.PropertyGroups.Any(x =>
            (x.Title == groupTitle.Trim() && x.Id != excludeId));
    }

    public OperationResult<int> EditPropertyGroup(EditPropertyGroupsViewmodel editPropertyGroup)
    {
        var propertyGroup = _context.PropertyGroups.FirstOrDefault(x => x.Id == editPropertyGroup.PropertyGroupId);
        if (propertyGroup == null)
            return OperationResult<int>.NotFound();

        if (IsPropertyGroupExist(editPropertyGroup.GroupTitle, editPropertyGroup.PropertyGroupId))
        {
            return OperationResult<int>.Duplicate();
        }

        propertyGroup.Title = editPropertyGroup.GroupTitle;
        propertyGroup.LastModified = DateTime.Now;

        _context.PropertyGroups.Update(propertyGroup);
        _context.SaveChanges();
        return OperationResult<int>.Success(propertyGroup.Id);

    }

    public EditPropertyGroupsViewmodel? GetPropertyGroupById(int propertyGroupId)
    {
        return _context.PropertyGroups.Where(x => x.Id == propertyGroupId)
            .Select(x => new EditPropertyGroupsViewmodel()
            {
                PropertyGroupId = x.Id,
                GroupTitle = x.Title,
            }).AsNoTracking().FirstOrDefault();
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