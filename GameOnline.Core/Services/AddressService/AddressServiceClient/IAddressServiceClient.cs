using GameOnline.Core.ViewModels.CartViewmodel.Client;
using GameOnline.Core.ViewModels.UserViewmodel.Client;

namespace GameOnline.Core.Services.AddressService.AddressServiceClient;

public interface IAddressServiceClient
{
    GetCartForShoppingViewmodel? GetCartForShopping(int userId);
    List<GetAddressForProfileViewmodel> GetAddressForProfile(int userId);
}