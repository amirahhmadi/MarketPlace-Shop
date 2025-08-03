using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Properties;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.PropertyService.PropertyNameService;

public class PropertyNameServiceAdmin : IPropertyNameServiceAdmin
{
    private readonly GameOnlineContext _context;
    public PropertyNameServiceAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreatePropertyName(CreatePropertyNameViewmodel createPropertyName)
    {
        if (IsPropertyNameExist(createPropertyName.GroupId, createPropertyName.PropertyNameTitle, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        PropertyName propertyName = new PropertyName()
        {
            Title = createPropertyName.PropertyNameTitle,
            GroupId = createPropertyName.GroupId,
            CreationDate = DateTime.Now,
            type = createPropertyName.type
        };
        _context.PropertyNames.Add(propertyName);
        _context.SaveChanges();
        AddPropertyNameForCategory(createPropertyName.Categories, propertyName.Id);
        return OperationResult<int>.Success(propertyName.Id);
    }

    public OperationResult<int> EditPropertyName(EditPropertyNameViewmodel editPropertyName)
    {
        var result = _context.PropertyNames.FirstOrDefault(x => x.Id == editPropertyName.PropertyNameId);
        if (result == null)
            return OperationResult<int>.NotFound();
        

        if (IsPropertyNameExist(editPropertyName.GroupId, editPropertyName.PropertyNameTitle, editPropertyName.PropertyNameId))
        {
            return OperationResult<int>.Duplicate();
        }

        result.GroupId = editPropertyName.GroupId;
        result.Title = editPropertyName.PropertyNameTitle;
        result.LastModified = DateTime.Now;

        _context.PropertyNames.Update(result);
        _context.SaveChanges();
        return OperationResult<int>.Success(result.Id);
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

    public OperationResult<int> RemovePropertyName(int propertyNameId)
    {
        var result = _context.PropertyNames.FirstOrDefault(x => x.Id == propertyNameId);
        if (result == null)
        {
            return OperationResult<int>.NotFound();
        }

        _context.PropertyNames.Remove(result);
        _context.SaveChanges();
        return OperationResult<int>.Success(propertyNameId);
    }

    public bool IsPropertyNameExist(int propertyGroupId, string propertyNameTitle, int excludeId)
    {
        return _context.PropertyNames.Any(x =>
            (x.Title == propertyNameTitle.Trim() && x.GroupId == propertyGroupId && x.Id != excludeId));
    }



    public OperationResult<int> AddPropertyNameForCategory(List<int> categoryId, int nameId)
    {
        List<PropertyNameCategory> propertyNameCategories = new List<PropertyNameCategory>();
        foreach (var item in categoryId)
        {
            propertyNameCategories.Add(new PropertyNameCategory()
            {
                CategoryId = item,
                CreationDate = DateTime.Now,
                PropertyNameId = nameId,
            });
        }
        _context.PropertyNameCategories.AddRange(propertyNameCategories);
        _context.SaveChanges();
        return OperationResult<int>.Success(nameId);
    }
}