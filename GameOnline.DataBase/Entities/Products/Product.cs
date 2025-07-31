using System.ComponentModel.DataAnnotations.Schema;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Categories;
using GameOnline.DataBase.Entities.Properties;

namespace GameOnline.DataBase.Entities.Products;

public class Product : BaseEntity
{
    public string ImageName { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public bool IsActive { get; set; }
    public int Score { get; set; }



    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }

    [ForeignKey(nameof(BrandId))]
    public Brand Brand { get; set; }

    public List<ProductGallery> ProductGalleries { get; set; }
    public List<ProductReview> ProductReviews { get; set; }
    public List<PropertyProduct> PropertyProducts { get; set; }

}