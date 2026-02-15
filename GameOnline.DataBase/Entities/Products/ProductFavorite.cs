using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Users;

namespace GameOnline.DataBase.Entities.Products;

public class ProductFavorite : BaseEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
}