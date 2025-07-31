using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Categories;

namespace GameOnline.DataBase.Entities.Properties;

public class PropertyNameCategory : BaseEntity
{
    public int PropertyNameId { get; set; }
    public int CategoryId { get; set; }


    [ForeignKey(nameof(PropertyNameId))]
    public PropertyName PropertyName { get; set; }


    [ForeignKey(nameof(CategoryId))] 
    public Category Category { get; set; }
}