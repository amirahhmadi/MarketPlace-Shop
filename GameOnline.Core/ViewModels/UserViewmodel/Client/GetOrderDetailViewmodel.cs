namespace GameOnline.Core.ViewModels.UserViewmodel.Client
{
    public class GetOrderDetailViewmodel
    {
        public int CartId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int Price { get; set; }
        public byte OrderType { get; set; }
        public OrderDeatilViewmodel OrderDetail { get; set; }
    }

    public class OrderDeatilViewmodel
    {
        public int ProductId { get; set; }
        public string FaTitle { get; set; }
        public string ImgName { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }

    }
}
