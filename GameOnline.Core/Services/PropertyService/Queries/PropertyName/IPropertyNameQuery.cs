using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.PropertyService.Queries.PropertyName;

public interface IPropertyNameQuery
{
    List<GetPropertyNameViewmodel> GetPropertyName();
    EditPropertyNameViewmodel? GetPropertyNameById(int propertyNameId);
    bool IsPropertyNameExist(int propertyGroupId, string propertyNameTitle, int excludeId);
}

public class PropertyNameQuery : IPropertyNameQuery
{
    private readonly GameOnlineContext _context;
    public PropertyNameQuery(GameOnlineContext context)
    {
        _context = context;
    }

    public List<GetPropertyNameViewmodel> GetPropertyName()
    {
        return (from n in _context.PropertyNames
            join g in _context.PropertyGroups on n.GroupId equals g.Id
            select new GetPropertyNameViewmodel()
            {
                GroupTitle = g.Title,
                PropertyNameId = n.Id,
                PropertyNameTitle = n.Title,
                type = n.type
            }).AsNoTracking().ToList();
    }

    public EditPropertyNameViewmodel? GetPropertyNameById(int propertyNameId)
    {
        return _context.PropertyNames
            .Where(x => x.Id == propertyNameId)
            .Select(x => new EditPropertyNameViewmodel()
            {
                PropertyNameId = x.Id,
                GroupId = x.GroupId,
                PropertyNameTitle = x.Title,
            }).AsNoTracking().FirstOrDefault();

    }

    public bool IsPropertyNameExist(int propertyGroupId, string propertyNameTitle, int excludeId)
    {
        return _context.PropertyNames.Any(x =>
            (x.Title == propertyNameTitle.Trim() && x.GroupId == propertyGroupId && x.Id != excludeId));
    }
}