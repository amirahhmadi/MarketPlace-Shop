namespace GameOnline.Core.ViewModels.UserViewmodel.Client
{
    public class GetOrdersViewmodel
    {
        public int CartId { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime CreationDate { get; set; }
        public int Price { get; set; }
        public byte OrderType { get; set; }
        public SumOrderProfileViewmodel sumOrder { get; set; }
    }
    public class SumOrderProfileViewmodel
    {
        public int Price { get; set; }
        public int Count { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModified { get; set; }

    }
}
