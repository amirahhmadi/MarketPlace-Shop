using GameOnline.Common.Core;
using GameOnline.Core.Services.PropertyService.Queries.PropertyName;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Properties;

namespace GameOnline.Core.Services.PropertyService.Commands.PropertyName;

public interface IPropertyNameCommand
{
    OperationResult<int> CreatePropertyName(CreatePropertyNameViewmodel createPropertyName);
    OperationResult<int> EditPropertyName(EditPropertyNameViewmodel editPropertyName);
    OperationResult<int> RemovePropertyName(int propertyNameId);
}

public class PropertyNameCommand : IPropertyNameCommand
{
    private readonly GameOnlineContext _context;
    private readonly IPropertyNameQuery _nameQuery;

    public PropertyNameCommand(GameOnlineContext context, IPropertyNameQuery nameQuery)
    {
        _context = context;
        _nameQuery = nameQuery;
    }

    public OperationResult<int> CreatePropertyName(CreatePropertyNameViewmodel createPropertyName)
    {
        if (_nameQuery.IsPropertyNameExist(createPropertyName.GroupId, createPropertyName.PropertyNameTitle, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        DataBase.Entities.Properties.PropertyName propertyName = new DataBase.Entities.Properties.PropertyName()
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

        if (_nameQuery.IsPropertyNameExist(editPropertyName.GroupId, editPropertyName.PropertyNameTitle, editPropertyName.PropertyNameId))
            return OperationResult<int>.Duplicate();

        // ویرایش اطلاعات ویژگی
        result.GroupId = editPropertyName.GroupId;
        result.Title = editPropertyName.PropertyNameTitle.Trim();
        result.LastModified = DateTime.Now;
        _context.PropertyNames.Update(result);

        // حذف دسته‌بندی‌های قدیمی
        var oldCategories = _context.PropertyNameCategories.Where(x => x.PropertyNameId == result.Id);
        _context.PropertyNameCategories.RemoveRange(oldCategories);

        // اضافه کردن دسته‌بندی‌های جدید
        if (editPropertyName.Categories != null && editPropertyName.Categories.Any())
        {
            var newCategories = editPropertyName.Categories.Select(catId => new PropertyNameCategory
            {
                PropertyNameId = result.Id,
                CategoryId = catId,
                CreationDate = DateTime.Now
            }).ToList();
            _context.PropertyNameCategories.AddRange(newCategories);
        }

        _context.SaveChanges();
        return OperationResult<int>.Success(result.Id);
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