using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ViewModels.BrandViewModels;

public class RemoveBrandsViewModel
{
    public int BrandId { get; set; }
    public string OldImgName { get; set; }
    public IFormFile? ImageName { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public string? Description { get; set; }
}