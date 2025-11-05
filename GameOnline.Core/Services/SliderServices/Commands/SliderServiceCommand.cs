using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using GameOnline.Core.Services.SliderServices.Queries;
using GameOnline.Core.ViewModels.SliderViewModels.Admin;
using GameOnline.DataBase.Context;
using GameOnline.DataBase.Entities.Sliders;

namespace GameOnline.Core.Services.SliderServices.Commands;

public class SliderServiceCommand : ISliderServiceCommand
{
    private readonly GameOnlineContext _context;
    private readonly ISliderServiceQuery _serviceQuery;
    public SliderServiceCommand(GameOnlineContext context, ISliderServiceQuery serviceQuery)
    {
        _context = context;
        _serviceQuery = serviceQuery;
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