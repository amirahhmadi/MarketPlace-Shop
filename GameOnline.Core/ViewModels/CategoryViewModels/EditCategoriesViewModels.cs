using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ViewModels.CategoryViewModels;

public class EditCategoriesViewModels
{
    public int CategoryId { get; set; }
    public List<int> ParentIds { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public string? OldImage { get; set; }
    public IFormFile? ImageName { get; set; }
    public bool IsActive { get; set; }
    public bool IsMine { get; set; }
    public AddOrEditParentCategoryViewmodel? AddOrEditParent { get; set; }

}