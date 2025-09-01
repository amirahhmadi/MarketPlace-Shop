using System.ComponentModel.DataAnnotations;

namespace GameOnline.Core.ViewModels.PropertyViewmodel.PropertyValueViewmodel
{
    public class AddOrUpdatePropertyValueForProductViewmodel
    {
        [Required(ErrorMessage = "شناسه‌های ویژگی‌ها الزامی است")]
        [MinLength(1, ErrorMessage = "حداقل یک ویژگی باید انتخاب شود")]
        public List<int> nameid { get; set; } = new();

        [Required(ErrorMessage = "مقادیر ویژگی‌ها الزامی است")]
        public List<string> value { get; set; } = new();

        [Required(ErrorMessage = "شناسه محصول الزامی است")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "شناسه دسته‌بندی الزامی است")]
        public int categoryid { get; set; }

        [Required(ErrorMessage = "نام ویژگی‌ها الزامی است")]
        public List<AddPropertyNameForProductViewmodel> propertyNameForProduct { get; set; } = new();
    }

    public class AddPropertyNameForProductViewmodel
    {
        [Required(ErrorMessage = "شناسه نام ویژگی الزامی است")]
        public int NameId { get; set; }

        [Required(ErrorMessage = "عنوان نام ویژگی الزامی است")]
        [MaxLength(200, ErrorMessage = "عنوان نام ویژگی نمی‌تواند بیشتر از 200 کاراکتر باشد")]
        public string PropertyNameTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع ویژگی الزامی است")]
        public byte type { get; set; }

        [Required(ErrorMessage = "مقادیر ویژگی الزامی است")]
        public List<GetPropertyValuesForPropertyNameViewmoedl> Values { get; set; } = new();
    }

    public class GetPropertyValuesForPropertyNameViewmoedl
    {
        [Required(ErrorMessage = "شناسه نام ویژگی الزامی است")]
        public int NameId { get; set; }

        [Required(ErrorMessage = "شناسه مقدار ویژگی الزامی است")]
        public int ValueId { get; set; }

        [Required(ErrorMessage = "مقدار ویژگی الزامی است")]
        [MaxLength(500, ErrorMessage = "مقدار ویژگی نمی‌تواند بیشتر از 500 کاراکتر باشد")]
        public string Value { get; set; } = string.Empty;
    }

    public class PropertyOldValueForProductViewmodel
    {
        [Required]
        public int PropertyValueId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public int ProductPropertyId { get; set; }

        [Required]
        public int NameId { get; set; }

        [Required]
        public int ValueId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Value { get; set; } = string.Empty;
    }

    public class GetPropertyNameByIdForAddProductViewmodel
    {
        [Required]
        public int NameId { get; set; }

        [Required]
        public byte type { get; set; }
    }
}
