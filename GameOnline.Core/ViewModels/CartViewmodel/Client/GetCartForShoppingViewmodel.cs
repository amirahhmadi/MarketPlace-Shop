namespace GameOnline.Core.ViewModels.CartViewmodel.Client
{
    public class GetCartForShoppingViewmodel
    {
        public int AddressId { get; set; }
        public string UserName { get; set; }
        public string? Phone { get; set; }
        public string PostalCode { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string FullAddress { get; set; }
        public List<GetCartDetailsViewmodel> GetCartDetails { get; set; }

    }
}
