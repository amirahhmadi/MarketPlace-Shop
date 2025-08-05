using Microsoft.AspNetCore.Http;

namespace GameOnline.Core.ViewModels.SliderViewModels.Admin;

public class CreateSlidersViewModel
{
    public IFormFile ImageName { get; set; }
    public string? Link { get; set; }
    public bool IsActive { get; set; }
    public int Sort { get; set; }
}