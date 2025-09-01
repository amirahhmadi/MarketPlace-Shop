namespace GameOnline.Core.ViewModels.UserViewmodel.Client
{
    public class GetOrderDetailViewmodel
    {
        public int CartId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int Price { get; set; }
        public int OriginalPrice { get; set; }
        public byte OrderType { get; set; }
        public OrderDeatilViewmodel OrderDetail { get; set; }
    }

    public class OrderDeatilViewmodel
    {
        public int ProductId { get; set; }
        public string FaTitle { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }

        // قیمت نهایی (بعد از تخفیف، همون چیزی که در CartDetail ذخیره شده)
        public int Price { get; set; }

        // قیمت اصلی (قبل از تخفیف)
        public int OriginalPrice { get; set; }
    }
}
