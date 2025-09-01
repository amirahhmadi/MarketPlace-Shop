using GameOnline.Core.ViewModels.PropertyViewmodel.PropertyGroupViewmodel;

namespace GameOnline.Core.ViewModels.PropertyViewmodel.PropertyNameViewmodel;

public class EditPropertyNameViewmodel
{
    public int PropertyNameId { get; set; }
    public string PropertyNameTitle { get; set; }
    public int GroupId { get; set; }
    public List<int> Categories { get; set; } = new();
    public byte type { get; set; }

    public List<GetPropertyGroupsViewmodel>? GetPropertyGroups { get; set; }
}