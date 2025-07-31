using System.ComponentModel.DataAnnotations.Schema;

namespace GameOnline.DataBase.Entities.Properties;

public class PropertyName : BaseEntity
{
    public string Title { get; set; }
    public int GroupId { get; set; }


    [ForeignKey(nameof(GroupId))]
    public PropertyGroup PropertyGroup { get; set; }

    public List<PropertyValue> PropertyValues { get; set; }

    public List<PropertyNameCategory> PropertyNameCategories { get; set; }
}