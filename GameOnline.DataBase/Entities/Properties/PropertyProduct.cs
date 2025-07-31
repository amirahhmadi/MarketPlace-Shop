using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Products;

namespace GameOnline.DataBase.Entities.Properties;

public class PropertyProduct : BaseEntity
{
    public int PropertyValueId { get; set; }
    public int ProductId { get; set; }

    [ForeignKey(nameof(PropertyValueId))]
    public PropertyValue PropertyValue { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
}