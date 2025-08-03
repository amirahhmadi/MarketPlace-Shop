using System.ComponentModel.DataAnnotations.Schema;

namespace GameOnline.DataBase.Entities.Products;

public class ProductGallery : BaseEntity
{
    public int? ProductId { get; set; }
    public string ImageName { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
}