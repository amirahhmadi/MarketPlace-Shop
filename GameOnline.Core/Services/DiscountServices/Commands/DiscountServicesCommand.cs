using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.DiscountServices.Queries;
using GameOnline.Core.ViewModels.DiscountViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Discounts;

namespace GameOnline.Core.Services.DiscountServices.Commands;

public class DiscountServicesCommand : IDiscountServicesCommand
{
    private readonly GameOnlineContext _context;
    private readonly IDiscountServicesQuery _servicesQuery;

    public DiscountServicesCommand(GameOnlineContext context, IDiscountServicesQuery servicesQuery)
    {
        _context = context;
        _servicesQuery = servicesQuery;
    }

    public OperationResult<int> CreateDiscount(CreateDiscountViewModels createDiscountViewModels)
    {
        if (_servicesQuery.IsDiscountExist(createDiscountViewModels.Code, 0))
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
}