using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameOnline.Core.ViewModels.ProductViewmodel.Admin;

public class CreateProductViewmodel
{
    [Required(ErrorMessage = "آپلود تصویر الزامی است.")]
    public IFormFile ImageName { get; set; }

    [Required(ErrorMessage = "نام فارسی الزامی است.")]
    public string FaTitle { get; set; }

    [Required(ErrorMessage = "نام انگلیسی الزامی است.")]
    public string EnTitle { get; set; }

    [Required(ErrorMessage = "انتخاب دسته بندی الزامی است.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "انتخاب برند الزامی است.")]
    public int BrandId { get; set; }

    public bool IsActive { get; set; }
    public int Score { get; set; }
    public List<GetCategoriesViewModels> GetCategories { get; set; }
    public List<GetBrandsViewModel> GetBrands { get; set; }
}