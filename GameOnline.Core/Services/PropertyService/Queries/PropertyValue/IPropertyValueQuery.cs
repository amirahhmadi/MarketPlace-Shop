using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.PropertyService.Queries.PropertyValue;

public interface IPropertyValueQuery
{
    List<GetPropertyValuesViewmodel> GetPropertyValues();
    EditPropertyValueViewmodel? GetPropertyValueById(int propertyValueId);
    List<AddPropertyNameForProductViewmodel> GetPropertyNameForProductByCategoryId(int categoryId);
    List<PropertyOldValueForProductViewmodel> oldPropertyValueForProduct(int productId);
    bool IsPropertyValueExist(int propertyValueId, int propertyNameId, string propertyValue);
    GetPropertyNameByIdForAddProductViewmodel getNameByIdForAddProduct(int propertyNameId);
    OperationResult<bool> CheckValueForPropertyName(int propertyNameId, int propertyValueId);
    OperationResult<bool> CheckPropertyNameForCategory(int propertyNameId, int categoryId);
}

public class PropertyValueQuery : IPropertyValueQuery
{
    private readonly GameOnlineContext _context;
    public PropertyValueQuery(GameOnlineContext context)
    {
        _context = context;
    }


    public List<GetPropertyValuesViewmodel> GetPropertyValues()
    {
        return (from pv in _context.PropertyValues
                join pn in _context.PropertyNames on pv.PropertyNameId equals pn.Id

                select new GetPropertyValuesViewmodel
                {
                    PropertyNameTitle = pn.Title,
                    PropertyValueId = pv.Id,
                    Value = pv.Value
                })
            .AsNoTracking()
            .ToList();
    }

    public bool IsPropertyValueExist(int propertyValueId, int propertyNameId, string propertyValue)
    {
        return _context.PropertyValues.Any(x => x.Value == propertyValue.ToLower().Trim() &&
                                                x.PropertyNameId == propertyNameId && x.Id != propertyValueId);
    }

    public EditPropertyValueViewmodel? GetPropertyValueById(int propertyValueId)
    {
        return _context.PropertyValues
            .Where(x => x.Id == propertyValueId)
            .Select(x => new EditPropertyValueViewmodel()
            {
                PropertyValueId = x.Id,
                NameId = x.PropertyNameId,
                PropertyValueTitle = x.Value
            }).AsNoTracking().FirstOrDefault();
    }

    public List<AddPropertyNameForProductViewmodel> GetPropertyNameForProductByCategoryId(int categoryId)
    {
        var propertyName = (from pc in _context.PropertyNameCategories
                join pn in _context.PropertyNames on pc.PropertyNameId equals pn.Id

                where (pc.CategoryId == categoryId)

                select new AddPropertyNameForProductViewmodel
                {
                    NameId = pn.Id,
                    PropertyNameTitle = pn.Title,
                    type = pn.type,
                })
            .AsNoTracking()
            .ToList();

        for (int i = 0; i < propertyName.Count(); i++)
        {
            if (propertyName[i].type == PropertyType.single_choice ||
                propertyName[i].type == PropertyType.multiple_choice)
            {
                propertyName[i].Values = _context.PropertyValues
                    .Where(x => x.PropertyNameId == propertyName[i].NameId)
                    .Select(x => new GetPropertyValuesForPropertyNameViewmoedl
                    {
                        NameId = x.PropertyNameId,
                        Value = x.Value,
                        ValueId = x.Id,
                    })
                    .AsNoTracking()
                    .ToList();
            }
        }

        return propertyName;
    }

    public List<PropertyOldValueForProductViewmodel> oldPropertyValueForProduct(int productId)
    {
        var OldValue = (from pProperty in _context.PropertyProducts
                join pv in _context.PropertyValues on pProperty.PropertyValueId equals pv.Id
                join pn in _context.PropertyNames on pv.PropertyNameId equals pn.Id

                where (pProperty.ProductId == productId)

                select new PropertyOldValueForProductViewmodel
                {
                    NameId = pn.Id,
                    Value = pv.Value,
                    ValueId = pv.Id,
                    ProductPropertyId = pProperty.Id,
                })
            .AsNoTracking()
            .ToList();
        return OldValue;
    }

    public GetPropertyNameByIdForAddProductViewmodel getNameByIdForAddProduct(int propertyNameId)
    {
        return _context.PropertyNames
            .Where(x => x.Id == propertyNameId)
            .Select(x => new GetPropertyNameByIdForAddProductViewmodel
            {
                NameId = x.Id,
                type = x.type,
            })
            .AsNoTracking()
            .SingleOrDefault();
    }

    public OperationResult<bool> CheckValueForPropertyName(int propertyNameId, int propertyValueId)
    {
        bool existValue = _context.PropertyValues
            .Any(x => x.PropertyNameId == propertyNameId && x.Id == propertyValueId);

        return new OperationResult<bool>
        {
            Data = existValue,
        };
    }

    public OperationResult<bool> CheckPropertyNameForCategory(int propertyNameId, int categoryId)
    {
        bool existPropName = _context.PropertyNameCategories
            .Any(x => x.PropertyNameId == propertyNameId && x.CategoryId == categoryId);

        return new OperationResult<bool>
        {
            Data = existPropName
        };
    }
}