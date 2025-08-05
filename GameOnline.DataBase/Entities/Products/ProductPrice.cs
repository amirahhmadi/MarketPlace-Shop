using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Carts;
using GameOnline.DataBase.Entities.Colors;
using GameOnline.DataBase.Entities.Guarantees;
using GameOnline.DataBase.Entities.Sellers;

namespace GameOnline.DataBase.Entities.Products;

public class ProductPrice :BaseEntity
{
    public int Price { get; set; }
    public int SpecialPrice { get; set; }
    public int Count { get; set; }
    public int? MaxOrderCount { get; set; }
    public int SubmitDate { get; set; }

    public int ProductId { get; set; }
    public int GuaranteeId { get; set; }
    public int ColorId { get; set; }
    public int SellerId { get; set; }


    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    [ForeignKey(nameof(GuaranteeId))]
    public Guarantee Guarantee { get; set; }

    [ForeignKey(nameof(ColorId))]
    public Color Color { get; set; }

    [ForeignKey(nameof(SellerId))]
    public Seller Seller { get; set; }

    public List<CartDetail> CartDetails { get; set; }
}