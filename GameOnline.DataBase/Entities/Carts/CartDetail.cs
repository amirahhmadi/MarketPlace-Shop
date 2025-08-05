using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Products;

namespace GameOnline.DataBase.Entities.Carts;

public class CartDetail : BaseEntity
{
    public int Count { get; set; }
    public int Price { get; set; }
    public int CartId { get; set; }
    public int ProductPriceId { get; set; }

    [ForeignKey(nameof(CartId))]
    public Cart Cart { get; set; }

    [ForeignKey(nameof(ProductPriceId))]
    public ProductPrice ProductPrice { get; set; }
}