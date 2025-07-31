using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;
using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;

namespace GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel;

public class EditPropertyValueViewmodel
{
    public int PropertyValueId { get; set; }
    public string PropertyValueTitle { get; set; }
    public int NameId { get; set; }
    public List<GetPropertyNameViewmodel>? GetPropertyName { get; set; }

}