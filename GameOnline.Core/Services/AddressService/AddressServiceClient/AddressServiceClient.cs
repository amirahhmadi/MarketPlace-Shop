using GameOnline.Core.Services.CartService.CartServiceClient;
using GameOnline.Core.ViewModels.CartViewmodel.Client;
using GameOnline.Core.ViewModels.UserViewmodel.Client;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.AddressService.AddressServiceClient;

public class AddressServiceClient : IAddressServiceClient
{
    private readonly GameOnlineContext _context;
    private readonly ICartServiceClient _cartService;
    public AddressServiceClient(GameOnlineContext context, ICartServiceClient cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    public List<GetAddressForProfileViewmodel> GetAddressForProfile(int userId)
    {
        var findUserAddress = (from ua in _context.UserAddresses
                join p in _context.Provinces on ua.ProvinceId equals p.Id
                join c in _context.Cities on ua.CityId equals c.Id

                where (ua.UserId == userId)

                select new GetAddressForProfileViewmodel
                {
                    AddressId = ua.Id,
                    CityName = c.CityName,
                    FullAddress = ua.FullAddress,
                    Phone = ua.Phone,
                    PostalCode = ua.PostalCode,
                    ProvinceName = p.ProvinceName,
                    UserName = ua.UserName,
                    IsActive = ua.IsActive
                })
            .AsNoTracking()
            .ToList();

        return findUserAddress;
    }

    public GetCartForShoppingViewmodel? GetCartForShopping(int userId)
    {
        var findActiveUserAddress = (from ua in _context.UserAddresses
                join p in _context.Provinces on ua.ProvinceId equals p.Id
                join c in _context.Cities on ua.CityId equals c.Id

                where (ua.UserId == userId)

                select new GetCartForShoppingViewmodel
                {
                    AddressId = ua.Id,
                    CityName = c.CityName,
                    FullAddress = ua.FullAddress,
                    Phone = ua.Phone,
                    PostalCode = ua.PostalCode,
                    ProvinceName = p.ProvinceName,
                    UserName = ua.UserName,

                })
            .AsNoTracking()
            .FirstOrDefault();

        return findActiveUserAddress;
    }
}