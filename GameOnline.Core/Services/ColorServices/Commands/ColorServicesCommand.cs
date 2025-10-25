using GameOnline.Common.Core;
using GameOnline.Core.Services.ColorServices.Queries;
using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Colors;

namespace GameOnline.Core.Services.ColorServices.Commands;

public class ColorServicesCommand : IColorServicesCommand
{
    private readonly GameOnlineContext _context;
    private readonly IColorServicesQuery _servicesQuery;

    public ColorServicesCommand(GameOnlineContext context, IColorServicesQuery servicesQuery)
    {
        _context = context;
        _servicesQuery = servicesQuery;
    }

    public OperationResult<int> CreateColor(CreateColorsViewModel createColors)
    {
        if (_servicesQuery.IsColorExist(createColors.ColorCode, createColors.ColorName, 0))
        {
            return OperationResult<int>.Duplicate();
        }

        Color color = new Color()
        {
            CreationDate = DateTime.Now,
            ColorName = createColors.ColorName,
            Code = createColors.ColorCode,
            IsActive = createColors.IsActive
        };
        _context.Colors.Add(color);
        _context.SaveChanges();
        return OperationResult<int>.Success(color.Id);
    }

    public OperationResult<int> EditColor(EditColorsViewModel editColors)
    {
        var color = _context.Colors.FirstOrDefault(x => x.Id == editColors.ColorId);
        if (color == null)
            return OperationResult<int>.NotFound();

        if (_servicesQuery.IsColorExist(editColors.ColorCode, editColors.ColorName, editColors.ColorId))
        {
            return OperationResult<int>.Duplicate();
        }

        color.ColorName = editColors.ColorName;
        color.Code = editColors.ColorCode;
        color.IsActive = editColors.IsActive;
        color.LastModified = DateTime.Now;

        _context.Colors.Update(color);
        _context.SaveChanges();
        return OperationResult<int>.Success(color.Id);
    }

    public OperationResult<int> RemoveColor(RemoveColorsViewModel removeColors)
    {
        var color = _context.Colors
            .FirstOrDefault(x => x.Id == removeColors.ColorId);

        _context.Colors.Remove(color);
        _context.SaveChanges();
        return OperationResult<int>.Success(removeColors.ColorId);
    }
}