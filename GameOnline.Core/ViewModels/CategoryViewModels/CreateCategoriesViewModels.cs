using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ViewModels.CategoryViewModels;

public class CreateCategoriesViewModels
{
    public int? ParentId { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageName { get; set; }
    public bool IsActive { get; set; }
    public bool IsMine { get; set; }
    public AddOrEditParentCategoryViewmodel? AddOrEditParent { get; set; }
}