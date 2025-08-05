using GameOnline.DataBase.Entities.Products;

namespace GameOnline.DataBase.Entities.Sellers;

public class Seller : BaseEntity
{
    public string SellerNmae { get; set; }
    public string Password { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; }
    public string? imageName { get; set; }

    public List<ProductPrice> ProductPrices { get; set; }

}