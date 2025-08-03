using GameOnline.Core.ViewModels.DiscountViewModels;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;

public class DiscountServicesAdmin : IDiscountServicesAdmin
{
    private readonly GameOnlineContext _context;
    public DiscountServicesAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public List<GetDiscountViewModels> GetDiscount()
    {
        return _context.Discounts.Select(x => new GetDiscountViewModels()
        {
            Code = x.Code,
            IsActive = x.IsActive,
            DiscountId = x.Id,
            EndDiscount = x.EndDiscount,
            StartDiscount = x.StartDiscount,
            UserCount = x.UserCount
        }).AsNoTracking().ToList();
    }
}