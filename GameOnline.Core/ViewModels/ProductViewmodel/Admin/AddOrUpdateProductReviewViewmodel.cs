using System.ComponentModel.DataAnnotations;

namespace GameOnline.Core.ViewModels.ProductViewmodel.Admin
{
    public class AddOrUpdateProductReviewViewmodel
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "شناسه محصول الزامی است")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "نوشتن متن بررسی الزامی است")]
        [MaxLength(1000, ErrorMessage = "بررسی نمی‌تواند بیشتر از 1000 کاراکتر باشد")]
        public string? Review { get; set; }

        [Required(ErrorMessage = "ثبت نکات مثبت الزامی است")]
        [MaxLength(500, ErrorMessage = "نکات مثبت نمی‌تواند بیشتر از 500 کاراکتر باشد")]
        public string? Positive { get; set; }

        [Required(ErrorMessage = "ثبت نکات منفی الزامی است")]
        [MaxLength(500, ErrorMessage = "نکات منفی نمی‌تواند بیشتر از 500 کاراکتر باشد")]
        public string? Negative { get; set; }
    }
}