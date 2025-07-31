using System.ComponentModel.DataAnnotations.Schema;

namespace GameOnline.DataBase.Entities.Properties;

public class PropertyValue : BaseEntity
{
    public string Value { get; set; }
    public int PropertyNameId { get; set; }

    [ForeignKey(nameof(PropertyNameId))]
    public PropertyName PropertyName { get; set; }

    public List<PropertyProduct> PropertyProducts { get; set; }
}