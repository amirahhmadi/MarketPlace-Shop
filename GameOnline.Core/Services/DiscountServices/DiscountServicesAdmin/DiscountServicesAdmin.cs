using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.DiscountViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Discounts;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.DiscountServices.DiscountServicesAdmin;

public class DiscountServicesAdmin : IDiscountServicesAdmin
{
    private readonly GameOnlineContext _context;
    public DiscountServicesAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreateDiscount(CreateDiscountViewModels createDiscountViewModels)
    {
        if (IsDiscountExist(createDiscountViewModels.Code, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        Discount discount = new Discount()
        {
            CreationDate = DateTime.Now,
            Code = createDiscountViewModels.Code,
            IsActive = createDiscountViewModels.IsActive,
            UserCount = createDiscountViewModels.UserCount,
            StartDiscount = createDiscountViewModels.StartDiscount.ShamsiToMiladi(),
            EndDiscount = createDiscountViewModels.EndDiscount.ShamsiToMiladi(),
        };
        _context.Discounts.Add(discount);
        _context.SaveChanges();
        return OperationResult<int>.Success(discount.Id);
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