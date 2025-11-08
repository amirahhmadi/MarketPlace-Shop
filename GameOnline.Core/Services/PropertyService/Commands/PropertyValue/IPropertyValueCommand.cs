using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.PropertyService.Queries.PropertyValue;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Properties;

namespace GameOnline.Core.Services.PropertyService.Commands.PropertyValue;

public interface IPropertyValueCommand
{
    OperationResult<int> CreatePropertyValue(CreatePropertyValueViewmodel propertyValue);
    OperationResult<int> EditPropertyValue(EditPropertyValueViewmodel editPropertyValue);
    OperationResult<int> RemovePropertyValue(int propertyValueId);
    OperationResult<int> AddOrRemovePropertyForProduct(AddOrUpdatePropertyValueForProductViewmodel addOrUpdateProperty);
}

public class PropertyValueCommand : IPropertyValueCommand
{
    private readonly GameOnlineContext _context;
    private readonly IPropertyValueQuery _valueQuery;

    public PropertyValueCommand(GameOnlineContext context, IPropertyValueQuery valueQuery)
    {
        _context = context;
        _valueQuery = valueQuery;
    }

    public OperationResult<int> CreatePropertyValue(CreatePropertyValueViewmodel propertyValue)
    {
        if (_valueQuery.IsPropertyValueExist(0, propertyValue.PropertyNameId, propertyValue.Value))
        {
            return OperationResult<int>.Duplicate();
        }

        DataBase.Entities.Properties.PropertyValue value = new DataBase.Entities.Properties.PropertyValue()
        {
            CreationDate = DateTime.Now,
            PropertyNameId = propertyValue.PropertyNameId,
            Value = propertyValue.Value,
        };

        _context.PropertyValues.Add(value);
        _context.SaveChanges();

        return OperationResult<int>.Success(value.Id);

    }

    public OperationResult<int> EditPropertyValue(EditPropertyValueViewmodel editModel)
    {
        var propertyValue = _context.PropertyValues
            .FirstOrDefault(x => x.Id == editModel.PropertyValueId);

        if (propertyValue == null)
            return OperationResult<int>.NotFound();

        var newValueNormalized = editModel.PropertyValueTitle.ToLower().Trim();
        var currentValueNormalized = propertyValue.Value.ToLower().Trim();

        // اگر نام و مقدار هیچ‌کدام تغییر نکرده‌اند، نیازی به ذخیره نیست
        if (propertyValue.PropertyNameId == editModel.NameId &&
            currentValueNormalized == newValueNormalized)
        {
            return OperationResult<int>.Success(propertyValue.Id);
        }

        // بررسی تکراری نبودن
        bool isDuplicate = _valueQuery.IsPropertyValueExist(
            editModel.NameId,
            editModel.PropertyValueId,
            editModel.PropertyValueTitle
        );

        if (isDuplicate)
            return OperationResult<int>.Duplicate();

        // بروزرسانی
        propertyValue.PropertyNameId = editModel.NameId;
        propertyValue.Value = editModel.PropertyValueTitle;
        propertyValue.LastModified = DateTime.Now;

        _context.SaveChanges();

        return OperationResult<int>.Success(propertyValue.Id);
    }

    public OperationResult<int> RemovePropertyValue(int propertyValueId)
    {
        var result = _context.PropertyValues.FirstOrDefault(x => x.Id == propertyValueId);
        if (result == null)
        {
            return OperationResult<int>.NotFound();
        }

        _context.PropertyValues.Remove(result);
        _context.SaveChanges();
        return OperationResult<int>.Success(propertyValueId);
    }

    public OperationResult<int> AddOrRemovePropertyForProduct(AddOrUpdatePropertyValueForProductViewmodel addOrUpdateProperty)
    {
        List<PropertyProduct> NewPropertyForProduct = new List<PropertyProduct>();
        List<DataBase.Entities.Properties.PropertyValue> UpdateValue = new List<DataBase.Entities.Properties.PropertyValue>();
        List<PropertyProduct> RemovePropertyForProduct = new List<PropertyProduct>();
        List<DataBase.Entities.Properties.PropertyValue> RemovePropertyValue = new List<DataBase.Entities.Properties.PropertyValue>();

        var old_value = _valueQuery.oldPropertyValueForProduct(addOrUpdateProperty.ProductId);

        for (int i = 0; i < addOrUpdateProperty.nameid.Count(); i++)
        {
            int type = _valueQuery.getNameByIdForAddProduct(addOrUpdateProperty.nameid[i]).type;

            if (type == PropertyType.single_choice || type == PropertyType.multiple_choice)
            {
                if (String.IsNullOrEmpty(addOrUpdateProperty.value[i]) == false)
                {
                    int ValueId = int.Parse(addOrUpdateProperty.value[i]);
                    if (ValueId > 0)
                    {
                        bool ExistValueForPropName =
                            _valueQuery.CheckValueForPropertyName(addOrUpdateProperty.nameid[i], ValueId).Data;

                        bool ExistPropNameForCategory =
                            _valueQuery.CheckPropertyNameForCategory(addOrUpdateProperty.nameid[i], addOrUpdateProperty.categoryid).Data;

                        if (ExistValueForPropName && ExistPropNameForCategory)
                        {
                            if (old_value.Any(x => x.ValueId == ValueId) == false)
                            {
                                NewPropertyForProduct.Add(new PropertyProduct
                                {
                                    CreationDate = DateTime.Now,
                                    ProductId = addOrUpdateProperty.ProductId,
                                    PropertyValueId = ValueId,
                                });

                            }
                        }
                    }
                }
            }
            else if (type == PropertyType.linear || type == PropertyType.written)
            {
                var FindOldValue = old_value
                    .Where(x => x.NameId == addOrUpdateProperty.nameid[i])
                    .FirstOrDefault();

                if (FindOldValue != null)
                {
                    if (String.IsNullOrEmpty(addOrUpdateProperty.value[i]) == false)
                    {
                        UpdateValue.Add(new DataBase.Entities.Properties.PropertyValue
                        {
                            Id = FindOldValue.ValueId,
                            CreationDate = FindOldValue.CreationDate,
                            LastModified = DateTime.Now,
                            PropertyNameId = addOrUpdateProperty.nameid[i],
                            Value = addOrUpdateProperty.value[i]

                        });
                    }
                    else
                    {
                        RemovePropertyForProduct.Add(
                            new PropertyProduct
                            {
                                Id = FindOldValue.ProductPropertyId
                            });

                        RemovePropertyValue.Add(new DataBase.Entities.Properties.PropertyValue
                        {
                            Id = FindOldValue.ValueId,
                        });

                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(addOrUpdateProperty.value[i]) == false)
                    {
                        CreatePropertyValueViewmodel NewPropertyValue = new CreatePropertyValueViewmodel()
                        {
                            PropertyNameId = addOrUpdateProperty.nameid[i],
                            Value = addOrUpdateProperty.value[i],

                        };
                        var PropertyValueId = CreatePropertyValue(NewPropertyValue);
                        if (PropertyValueId.Data > 0)
                        {
                            NewPropertyForProduct.Add(new PropertyProduct
                            {
                                CreationDate = DateTime.Now,
                                ProductId = addOrUpdateProperty.ProductId,
                                PropertyValueId = PropertyValueId.Data,
                            });
                        }
                    }
                }

                old_value.Remove(FindOldValue);
            }

        }


        var level_1 = addOrUpdateProperty.value.Select(x => int.TryParse(x, out int result));
        var level_2 = addOrUpdateProperty.value.Select(x => int.TryParse(x, out int result) ? result : (int?)null);
        var level_3 = addOrUpdateProperty.value.Select(x => int.TryParse(x, out int result) ? result : (int?)null).Where(x => x.HasValue);
        var level_4 = addOrUpdateProperty.value.Select(x => int.TryParse(x, out int result) ? result : (int?)null)
            .Where(x => x.HasValue).Select(x => x.Value);

        List<int> num_1 = new List<int>()
            {
                1,2,3,4
            };

        List<int> num_2 = new List<int>()
            {
                1,2,3,4,5
            };
        var t = num_2.Except(num_1).ToList();

        level_4 = old_value.Select(x => x.ValueId).ToList()
                    .Except(level_4).ToList();

        foreach (var level in level_4)
        {

            RemovePropertyForProduct.Add(new PropertyProduct
            {
                Id = old_value.Where(x => x.ValueId == level)
                .FirstOrDefault().ProductPropertyId
            });
        }

        if (RemovePropertyForProduct.Count() > 0)
            _context.PropertyProducts.RemoveRange(RemovePropertyForProduct);

        if (RemovePropertyValue.Count() > 0)
            _context.PropertyValues.RemoveRange(RemovePropertyValue);

        if (NewPropertyForProduct.Count() > 0)
            _context.PropertyProducts.AddRange(NewPropertyForProduct);

        if (UpdateValue.Count() > 0)
            _context.PropertyValues.UpdateRange(UpdateValue);

        _context.SaveChanges();

        return OperationResult<int>.Success(addOrUpdateProperty.ProductId);

    }

}