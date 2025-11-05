using GameOnline.Core.ViewModels.DiscountViewModels;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.DiscountServices.Queries;

public class DiscountServicesQuery : IDiscountServicesQuery
{
    private readonly GameOnlineContext _context;
    public DiscountServicesQuery(GameOnlineContext context)
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

    public bool IsDiscountExist(string code, int excludeId)
    {
        return _context.Discounts.Any(x =>
            x.Code == code.Trim() &&
            x.Id != excludeId &&
            !x.IsRemove);
    }
}