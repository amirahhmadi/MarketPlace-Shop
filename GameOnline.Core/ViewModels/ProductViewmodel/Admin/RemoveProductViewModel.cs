using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ViewModels.ProductViewmodel.Admin;

public class RemoveProductViewModel
{
    public int ProductId { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public int Score { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public IFormFile? ImageName { get; set; }
    public string? OldImage { get; set; }
    public bool IsActive { get; set; }
    public List<GetBrandsViewModel>? GetBrands { get; set; }
    public List<GetCategoriesViewModels>? GetCategories { get; set; }
}