using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.Core.ViewModels.GuaranteeViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Colors;
using GameOnline.DataBase.Entities.Guarantees;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.ColorServices.ColorServicesAdmin;

public class ColorServicesAdmin : IColorServicesAdmin
{
    private readonly GameOnlineContext _context;

    public ColorServicesAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreateColor(CreateColorsViewModel createColors)
    {
        if (IsColorExist(createColors.ColorCode, createColors.ColorName, 0))
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

        if (IsColorExist(editColors.ColorCode, editColors.ColorName, editColors.ColorId))
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

    public EditColorsViewModel? GetColorById(int colorId)
    {
        return _context.Colors.Where(x => x.Id == colorId)
            .Select(x => new EditColorsViewModel()
            {
                ColorId = x.Id,
                ColorName = x.ColorName,
                ColorCode = x.Code,
                IsActive = x.IsActive
            }).AsNoTracking().FirstOrDefault();
    }

    public List<GetColorsViewModel> GetColors()
    {
        return _context.Colors.Select(x => new GetColorsViewModel()
        {
            ColorId = x.Id,
            ColorName = x.ColorName,
            ColorCode = x.Code,
            IsActive = x.IsActive
        }).AsNoTracking().ToList();
    }

    public bool IsColorExist(string colorCode, string colorName, int excludeId)
    {
        return _context.Colors.Any(x =>
            (x.ColorName == colorName.Trim() || x.Code == colorCode.Trim()) &&
            x.Id != excludeId);
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