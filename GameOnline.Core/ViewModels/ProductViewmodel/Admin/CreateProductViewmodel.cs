using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ViewModels.ProductViewmodel.Admin;

public class CreateProductViewmodel
{
    public IFormFile ImageName { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public bool IsActive { get; set; }
    public int Score { get; set; }
    public List<GetCategoriesViewModels> GetCategories { get; set; }
    public List<GetBrandsViewModel> GetBrands { get; set; }
}