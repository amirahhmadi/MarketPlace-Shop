using GameOnline.Common.Core;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Guarantees;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.GuaranteeServices.GuaranteeServicesAdmin;

public class GuaranteeServiceAdmin : IGuaranteeServiceAdmin
{
    private readonly GameOnlineContext _context;

    public GuaranteeServiceAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreateGuarantee(CreateGuaranteesViewModel createGuarantee)
    {
        if (IsGuaranteeExist(createGuarantee.GuaranteeName, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        Guarantee guarantee = new Guarantee()
        {
            CreationDate = DateTime.Now,
            GuaranteeName = createGuarantee.GuaranteeName,
            IsRemove = false
        };
        _context.Guarantees.Add(guarantee);
        _context.SaveChanges();
        return OperationResult<int>.Success(guarantee.Id);
    }

    public OperationResult<int> EditGuarantee(EditGuaranteesViewModel editGuarantee)
    {
        var guarantee = _context.Guarantees.FirstOrDefault(x => x.Id == editGuarantee.GuaranteeId);
        if (guarantee == null)
            return OperationResult<int>.NotFound();

        if (IsGuaranteeExist(editGuarantee.GuaranteeName, editGuarantee.GuaranteeId))
        {
            return OperationResult<int>.Duplicate();
        }

        guarantee.GuaranteeName = editGuarantee.GuaranteeName;
        guarantee.LastModified = DateTime.Now;

        _context.Guarantees.Update(guarantee);
        _context.SaveChanges();
        return OperationResult<int>.Success(guarantee.Id);

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

    public OperationResult<int> RemoveGuarantee(RemoveGuaranteesViewModel removeGuarantee)
    {
        var guarantee = _context.Guarantees
            .FirstOrDefault(x => x.Id == removeGuarantee.GuaranteeId);

        if (guarantee == null)
            return OperationResult<int>.NotFound();

        guarantee.IsRemove = true;
        guarantee.RemoveDate = DateTime.Now;

        _context.Guarantees.Update(guarantee);
        _context.SaveChanges();
        return OperationResult<int>.Success(removeGuarantee.GuaranteeId);
    }
}