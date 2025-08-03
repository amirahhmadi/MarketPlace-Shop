namespace GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;

public class AddOrUpdatePropertyValueForProductViewmodel
{
    public List<int> nameid { get; set; }
    public List<string> value { get; set; }
    public int ProductId { get; set; }
    public int categoryid { get; set; }
    public List<AddPropertyNameForProductViewmodel> propertyNameForProduct { get; set; }
}

public class AddPropertyNameForProductViewmodel
{
    public int NameId { get; set; }
    public string PropertyNameTitle { get; set; }
    public byte type { get; set; }
    public List<GetPropertyValuesForPropertyNameViewmoedl> Values { get; set; }
}

public class GetPropertyValuesForPropertyNameViewmoedl
{
    public int NameId { get; set; }
    public int ValueId { get; set; }
    public string Value { get; set; }
}

public class PropertyOldValueForProductViewmodel
{
    public int PropertyValueId { get; set; }
    public DateTime CreationDate { get; set; }
    public int ProductPropertyId { get; set; }
    public int NameId { get; set; }
    public int ValueId { get; set; }
    public string Value { get; set; }
}

public class GetPropertyNameByIdForAddProductViewmodel
{
    public int NameId { get; set; }
    public byte type { get; set; }
}