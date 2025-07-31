using GameOnline.Common.Core;
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


            if (IsPropertyValueExist(editPropertyValue.NameId,editPropertyValue.PropertyValueId, editPropertyValue.PropertyValueTitle))
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

    }
}
