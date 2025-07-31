namespace GameOnline.Core.ViewModels.CategoryViewModels;

public class GetCategoriesViewModels
{
    public int CategoryId { get; set; }
    public string FaTitle { get; set; }
    public string EnTitle { get; set; }
    public string? ImageName { get; set; }
    public bool IsActive { get; set; }
    public bool IsMine { get; set; }
}