using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ViewModels.SliderViewModels.Admin;

public class EditSlidersViewModel
{
    public int SliderId { get; set; }
    public IFormFile? ImageName { get; set; }
    public string OldImgName { get; set; }
    public string? Link { get; set; }
    public int Sort { get; set; }
    public bool IsActive { get; set; }
}