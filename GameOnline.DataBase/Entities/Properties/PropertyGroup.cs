namespace GameOnline.DataBase.Entities.Properties;

public class PropertyGroup : BaseEntity
{
    public string Title { get; set; }

    public List<PropertyName> PropertyNames { get; set; }
}