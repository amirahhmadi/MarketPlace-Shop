using GameOnline.Common.Core;
using GameOnline.Core.Services.GuaranteeServices.Queries;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Guarantees;

namespace GameOnline.Core.Services.GuaranteeServices.Commands;

public class GuaranteeServiceCommand : IGuaranteeServiceCommand
{
    private readonly GameOnlineContext _context;
    private readonly IGuaranteeServiceQuery _serviceQuery;
    public GuaranteeServiceCommand(GameOnlineContext context, IGuaranteeServiceQuery serviceQuery)
    {
        _context = context;
        _serviceQuery = serviceQuery;
    }

    public OperationResult<int> CreateGuarantee(CreateGuaranteesViewModel createGuarantee)
    {
        if (_serviceQuery.IsGuaranteeExist(createGuarantee.GuaranteeName, 0))
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

        if (_serviceQuery.IsGuaranteeExist(editGuarantee.GuaranteeName, editGuarantee.GuaranteeId))
        {
            return OperationResult<int>.Duplicate();
        }

        guarantee.GuaranteeName = editGuarantee.GuaranteeName;
        guarantee.LastModified = DateTime.Now;

        _context.Guarantees.Update(guarantee);
        _context.SaveChanges();
        return OperationResult<int>.Success(guarantee.Id);

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