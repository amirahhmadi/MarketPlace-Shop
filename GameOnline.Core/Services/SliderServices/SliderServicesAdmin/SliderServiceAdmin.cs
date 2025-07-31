using System.Security.Cryptography.X509Certificates;
using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.ViewModels.BrandViewModels;
using GameOnline.Core.ViewModels.SliderViewModels;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Brands;
using GameOnline.DataBase.Entities.Sliders;
using Microsoft.EntityFrameworkCore;

namespace GameOnline.Core.Services.SliderServices.SliderServicesAdmin;

public class SliderServiceAdmin : ISliderServiceAdmin
{
    private readonly GameOnlineContext _context;

    public SliderServiceAdmin(GameOnlineContext context)
    {
        _context = context;
    }

    public OperationResult<int> CreateSlider(CreateSlidersViewModel createSlider)
    {
        string imageName = createSlider.ImageName.UploadImage(PathTools.PathSliderImageAdmin);

        Slider slider = new Slider()
        {
            CreationDate = DateTime.Now,
            ImageName = imageName,
            IsActive = createSlider.IsActive,
            Link = createSlider.Link,
            Sort = createSlider.Sort
        };
        _context.Sliders.Add(slider);
        _context.SaveChanges();
        return OperationResult<int>.Success(slider.Id);
    }

    public OperationResult<int> EditSlider(EditSlidersViewModel editSlider)
    {
        var slider = _context.Sliders
            .FirstOrDefault(x => x.Id == editSlider.SliderId && x.IsRemove == false);

        if (slider == null)
            return OperationResult<int>.NotFound();

        if (editSlider.ImageName is { Length: > 0 })
        {
            ImageExtension.RemoveImage(slider.ImageName, PathTools.PathSliderImageAdmin);
            slider.ImageName = editSlider.ImageName.UploadImage(PathTools.PathSliderImageAdmin);
        }

        slider.Link = editSlider.Link;
        slider.IsActive = editSlider.IsActive;
        slider.Sort = editSlider.Sort;
        slider.LastModified = DateTime.Now;

        _context.Sliders.Update(slider);
        _context.SaveChanges();
        return OperationResult<int>.Success(editSlider.SliderId);
    }

    public EditSlidersViewModel? GetSliderById(int sliderId)
    {
        return _context.Sliders
            .Where(x => x.Id == sliderId)
            .Select(x => new EditSlidersViewModel()
            {
                SliderId = x.Id,
                OldImgName = x.ImageName,
                IsActive = x.IsActive,
                Link = x.Link,
                Sort = x.Sort
            }).AsNoTracking().SingleOrDefault();
    }

    public List<GetSlidersViewModel> GetSliders()
    {
        return _context.Sliders.Select(x => new GetSlidersViewModel()
        {
            ImageName = x.ImageName,
            IsActive = x.IsActive,
            SliderId = x.Id
        }).AsNoTracking().ToList();
    }

    public bool IsSliderExist(string link, string imageName, int excludeId)
    {
        return _context.Brands.Any(x =>
            (x.FaTitle == link.Trim() || x.EnTitle == imageName.Trim()) &&
            x.Id != excludeId);
    }

    public OperationResult<int> RemoveSlider(RemoveSlidersViewModel removeSlider)
    {
        var slider = _context.Sliders
            .FirstOrDefault(x => x.Id == removeSlider.SliderId);

        if (slider == null)
            return OperationResult<int>.NotFound();

        slider.IsRemove = true;
        slider.RemoveDate = DateTime.Now;

        _context.Sliders.Update(slider);
        _context.SaveChanges();
        return OperationResult<int>.Success(removeSlider.SliderId);
    }
}