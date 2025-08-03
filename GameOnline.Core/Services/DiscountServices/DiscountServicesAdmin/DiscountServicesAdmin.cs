using GameOnline.DataBase.Context;

namespace GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;

public class DiscountServicesAdmin : IDiscountServicesAdmin
{
    private readonly GameOnlineContext _context;
    public DiscountServicesAdmin(GameOnlineContext context)
    {
        _context = context;
    }
}