namespace GameOnline.Core.ViewModels.DiscountViewModels;

public class GetDiscountViewModels
{
    public int DiscountId { get; set; }
    public string Code { get; set; }
    public int? UserCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartDiscount { get; set; }
    public DateTime? EndDiscount { get; set; }
}