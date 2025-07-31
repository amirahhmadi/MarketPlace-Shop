using System.ComponentModel.DataAnnotations.Schema;

namespace GameOnline.DataBase.Entities.Products;

public class ProductReview : BaseEntity
{
    public int ProductId { get; set; }
    public string Review { get; set; }
    public string Positive { get; set; }
    public string Negative { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
}