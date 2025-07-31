using System.Collections;

namespace GameOnline.Core.ViewModels.ColorViewModels;

public class GetColorsViewModel
{
    public int ColorId { get; set; }
    public string ColorName { get; set; }
    public string ColorCode { get; set; }
    public bool IsActive { get; set; }
}