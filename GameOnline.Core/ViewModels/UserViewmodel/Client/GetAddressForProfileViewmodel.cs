namespace GameOnline.Core.ViewModels.UserViewmodel.Client
{
    public class GetAddressForProfileViewmodel
    {
        public int AddressId { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string? FullAddress { get; set; }
        public bool IsActive { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string UserName { get; set; }
    }
}
