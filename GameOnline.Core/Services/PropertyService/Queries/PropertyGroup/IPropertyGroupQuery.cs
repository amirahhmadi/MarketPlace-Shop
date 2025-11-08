using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.PropertyService.Queries.PropertyGroup;

public interface IPropertyGroupQuery
{
    List<GetPropertyGroupsViewmodel> GetPropertyGroups();
    EditPropertyGroupsViewmodel? GetPropertyGroupById(int propertyGroupId);
    bool IsPropertyGroupExist(string groupTitle, int excludeId);
}

public class PropertyGroupQuery : IPropertyGroupQuery
{
    private readonly GameOnlineContext _context;
    public PropertyGroupQuery(GameOnlineContext context)
    {
        _context = context;
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
}