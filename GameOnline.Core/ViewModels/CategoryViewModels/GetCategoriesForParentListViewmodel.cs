namespace GameOnline.Core.ViewModels.CategoryViewModels;

public class GetCategoriesForParentListViewmodel
{
    public int CategoryId { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public string? ImageName { get; set; }
    public bool IsActive { get; set; }
    public bool IsMine { get; set; }
}