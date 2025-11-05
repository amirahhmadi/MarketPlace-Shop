using GameOnline.Core.ViewModels.GuaranteeViewModels;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.GuaranteeServices.Queries;

public class GuaranteeServiceQuery : IGuaranteeServiceQuery
{
    private readonly GameOnlineContext _context;

    public GuaranteeServiceQuery(GameOnlineContext context)
    {
        _context = context;
    }
    public EditGuaranteesViewModel? GetGuaranteeById(int guaranteeId)
    {
        return _context.Guarantees.Where(x => x.Id == guaranteeId)
            .Select(x => new EditGuaranteesViewModel()
            {
                GuaranteeId = x.Id,
                GuaranteeName = x.GuaranteeName,
            }).AsNoTracking().FirstOrDefault();
    }

    public List<GetGuaranteesViewModel> GetGuarantees()
    {
        return _context.Guarantees.Select(x => new GetGuaranteesViewModel()
        {
            GuaranteeId = x.Id,
            GuaranteeName = x.GuaranteeName
        }).AsNoTracking().ToList();
    }

    public bool IsGuaranteeExist(string guaranteeName, int excludeId)
    {
        return _context.Guarantees.Any(x =>
            x.GuaranteeName == guaranteeName.Trim() &&
            x.Id != excludeId &&
            !x.IsRemove);
    }
}