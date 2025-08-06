namespace GameOnline.Core.ViewModels.ProductViewmodel.Admin;

public class AddOrUpdateProductReviewViewmodel
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }
    public string? Review { get; set; }
    public string? Positive { get; set; }
    public string? Negative { get; set; }
}