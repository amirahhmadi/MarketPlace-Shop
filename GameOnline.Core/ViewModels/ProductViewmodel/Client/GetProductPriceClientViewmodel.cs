namespace GameOnline.Core.ViewModels.ProductViewmodel.Client;

public class GetProductPriceClientViewmodel
{
    public int ProductPriceId { get; set; }
    public string GuaranteeName { get; set; }
    public int GuaranteeId { get; set; }
    public string ColorCode { get; set; }
    public int ColorId { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public int? SpecialPrice { get; set; }
    public int? MaxOrderCount { get; set; }
    public int? SubmitDate { get; set; }
    public int SellerId { get; set; }
    public int MainPrice { get; set; }
    public int? NewPrice { get; set; }
    public DateTime? StartDisCount { get; set; }
    public DateTime? EndDisCount { get; set; }
    public int? FinalPrice { get; set; }
    public bool HasDiscount { get; set; }
}

public class GetSellerClientViewmodel
{
    public int SellerId { get; set; }
    public string SellerName { get; set; }
    public string? imageName { get; set; }
}