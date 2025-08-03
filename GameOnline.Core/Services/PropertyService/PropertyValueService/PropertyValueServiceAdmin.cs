using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Properties;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.PropertyService.PropertyValueService
{
    public class PropertyValueServiceAdmin : IPropertyValueServiceAdmin
    {

        #region ctor
        private readonly GameOnlineContext _context;
        public PropertyValueServiceAdmin(GameOnlineContext context)
        {
            _context = context;
        }
        #endregion

        #region Get All PropertyValue
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
        #endregion

        #region Create PropertyValue
        public OperationResult<int> CreatePropertyValue(CreatePropertyValueViewmodel propertyValue)
        {
            if (IsPropertyValueExist(0, propertyValue.PropertyNameId, propertyValue.Value))
            {
                return OperationResult<int>.Duplicate();
            }

            PropertyValue value = new PropertyValue()
            {
                CreationDate = DateTime.Now,
                PropertyNameId = propertyValue.PropertyNameId,
                Value = propertyValue.Value,
            };

            _context.PropertyValues.Add(value);
            _context.SaveChanges();

            return OperationResult<int>.Success(value.Id);

        }
        #endregion

        #region Exist PropertyValue
        public bool IsPropertyValueExist(int propertyValueId, int propertyNameId, string propertyValue)
        {
            return _context.PropertyValues.Any(x => x.Value == propertyValue.ToLower().Trim() &&
            x.PropertyNameId == propertyNameId && x.Id != propertyValueId);
        }

        public OperationResult<int> EditPropertyValue(EditPropertyValueViewmodel editPropertyValue)
        {
            var result = _context.PropertyValues.FirstOrDefault(x => x.Id == editPropertyValue.PropertyValueId);
            if (result == null)
                return OperationResult<int>.NotFound();


            if (IsPropertyValueExist(editPropertyValue.NameId, editPropertyValue.PropertyValueId, editPropertyValue.PropertyValueTitle))
            {
                return OperationResult<int>.Duplicate();
            }

            result.PropertyNameId = editPropertyValue.NameId;
            result.Value = editPropertyValue.PropertyValueTitle;
            result.LastModified = DateTime.Now;

            _context.PropertyValues.Update(result);
            _context.SaveChanges();
            return OperationResult<int>.Success(result.Id);
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

        #endregion


        #region Property For Product
        public List<AddPropertyNameForProductViewmodel> GetPropertyNameForProductByCategoryId(int CategoryId)
        {
            var propertyName = (from pc in _context.PropertyNameCategories
                                join pn in _context.PropertyNames on pc.PropertyNameId equals pn.Id

                                where (pc.CategoryId == CategoryId)

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

        public List<PropertyOldValueForProductViewmodel> oldPropertyValueForProduct(int ProductId)
        {
            var OldValue = (from pProperty in _context.PropertyProducts
                            join pv in _context.PropertyValues on pProperty.PropertyValueId equals pv.Id
                            join pn in _context.PropertyNames on pv.PropertyNameId equals pn.Id

                            where (pProperty.ProductId == ProductId)

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

        public OperationResult<int> AddOrRemovePropertyForProduct(AddOrUpdatePropertyValueForProductViewmodel addOrUpdateProperty)
        {
            List<PropertyProduct> NewPropertyForProduct = new List<PropertyProduct>();
            List<PropertyValue> UpdateValue = new List<PropertyValue>();
            List<PropertyProduct> RemovePropertyForProduct = new List<PropertyProduct>();
            List<PropertyValue> RemovePropertyValue = new List<PropertyValue>();

            var old_value = oldPropertyValueForProduct(addOrUpdateProperty.ProductId);

            for (int i = 0; i < addOrUpdateProperty.nameid.Count(); i++)
            {
                int type = getNameByIdForAddProduct(addOrUpdateProperty.nameid[i]).type;

                if (type == PropertyType.single_choice || type == PropertyType.multiple_choice)
                {
                    if (String.IsNullOrEmpty(addOrUpdateProperty.value[i]) == false)
                    {
                        int ValueId = int.Parse(addOrUpdateProperty.value[i]);
                        if (ValueId > 0)
                        {
                            bool ExistValueForPropName =
                                CheckValueForPropertyName(addOrUpdateProperty.nameid[i], ValueId).Data;

                            bool ExistPropNameForCategory =
                                CheckPropertyNameForCategory(addOrUpdateProperty.nameid[i], addOrUpdateProperty.categoryid).Data;

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
                            UpdateValue.Add(new PropertyValue
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

                            RemovePropertyValue.Add(new PropertyValue
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

        public GetPropertyNameByIdForAddProductViewmodel getNameByIdForAddProduct(int PropertyNameId)
        {
            return _context.PropertyNames
                            .Where(x => x.Id == PropertyNameId)
                            .Select(x => new GetPropertyNameByIdForAddProductViewmodel
                            {
                                NameId = x.Id,
                                type = x.type,
                            })
                            .AsNoTracking()
                            .SingleOrDefault();
        }

        public OperationResult<bool> CheckValueForPropertyName(int PropertyNameId, int PropertyValueId)
        {
            bool ExistValue = _context.PropertyValues
                .Any(x => x.PropertyNameId == PropertyNameId && x.Id == PropertyValueId);

            return new OperationResult<bool>
            {
                Data = ExistValue,
            };
        }

        public OperationResult<bool> CheckPropertyNameForCategory(int PropertyNameId, int CategoryId)
        {
            bool ExistPropName = _context.PropertyNameCategories
                .Any(x => x.PropertyNameId == PropertyNameId && x.CategoryId == CategoryId);

            return new OperationResult<bool>
            {
                Data = ExistPropName
            };
        }

        #endregion
    }
}
