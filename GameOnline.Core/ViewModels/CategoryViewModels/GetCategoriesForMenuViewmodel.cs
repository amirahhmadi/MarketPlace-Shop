namespace GameOnline.Core.ViewModels.CategoryViewModels;

public class GetCategoriesForMenuViewmodel
{
    public int CategoryId { get; set; }
    public string FaTitle { get; set; }
    public bool IsMine { get; set; }
    public int? ParentId { get; set; }
}