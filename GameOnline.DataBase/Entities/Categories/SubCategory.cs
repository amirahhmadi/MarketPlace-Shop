using System.ComponentModel.DataAnnotations.Schema;

namespace GameOnline.DataBase.Entities.Categories;

public class SubCategory : BaseEntity
{
    public int ParentId { get; set; }
    public int SubId { get; set; }


    [ForeignKey(nameof(ParentId))]
    [InverseProperty("PSubCategory")]
    public Category Parent { get; set; }

    [ForeignKey(nameof(SubId))]
    [InverseProperty("SSubCategory")]
    public Category Sub { get; set; }
}