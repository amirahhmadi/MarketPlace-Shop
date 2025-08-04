namespace GameOnline.Core.ViewModels.DiscountViewModels;

public class CreateDiscountViewModels
{
    public string Code { get; set; }
    public int? UserCount { get; set; }
    public bool IsActive { get; set; }
    public string? StartDiscount { get; set; }
    public string? EndDiscount { get; set; }
}