using GameOnline.Core.ViewModels.ColorViewModels;
using GameOnline.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.ColorServices.Queries;

public class ColorServicesQuery : IColorServicesQuery
{
    private readonly GameOnlineContext _context;

    public ColorServicesQuery(GameOnlineContext context)
    {
        _context = context;
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
}