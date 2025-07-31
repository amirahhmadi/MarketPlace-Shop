namespace GameOnline.Core.ViewModels.SliderViewModels;

public class RemoveSlidersViewModel
{
    public int SliderId { get; set; }
    public string OldImgName { get; set; }
    public string? Link { get; set; }
    public int Sort { get; set; }
    public bool IsActive { get; set; }
}