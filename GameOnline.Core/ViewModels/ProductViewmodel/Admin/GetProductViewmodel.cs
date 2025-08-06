namespace GameOnline.Core.ViewModels.ProductViewmodel.Admin;

public class GetProductViewmodel
{
    public int ProductId { get; set; }
    public string ImageName { get; set; }
    public string FaTitle { get; set; }
    public string BrandName { get; set; }
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
}