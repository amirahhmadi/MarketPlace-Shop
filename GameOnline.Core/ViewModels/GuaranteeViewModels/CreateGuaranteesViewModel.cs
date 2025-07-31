using System.ComponentModel.DataAnnotations;

namespace GameOnline.Core.ViewModels.GuaranteeViewModels;

public class CreateGuaranteesViewModel
{
    [Display(Name = "نام گارانتی")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string GuaranteeName { get; set; }
}