namespace GameOnline.Core.ViewModels.CartViewmodel.Client
{
    public class GetCartDetailsViewmodel
    {
        public int? SpecialPrice { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int CartDetailId { get; set; }
        public int ProductPriceId { get; set; }
        public int CartCount { get; set; }
        public int Price { get; set; }
        public int MainPrice { get; set; }
        public int ProductCount { get; set; }
        public int? ProductMaxUserCount { get; set; }
        public string GuaranteeName { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public string ProductFaTitle { get; set; }
        public string ProductEnTitle { get; set; }
        public string SellerName { get; set; }
        public string ProductImg { get; set; }
        public string? Message { get; set; }
        public int? NewPrice { get; set; }
        public bool IsRemove { get; set; }
        public byte DetailType { get; set; }
        public int Score { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public DateTime? StartDisCount { get; set; }
        public DateTime? EndDisCount { get; set; }
    }
}
