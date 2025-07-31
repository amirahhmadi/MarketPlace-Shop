using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOnline.DataBase.Entities.Products;
using GameOnline.DataBase.Entities.Properties;

namespace GameOnline.DataBase.Entities.Categories
{
    public class Category : BaseEntity
    {
        public string FaTitle { get; set; }
        public string EnTitle { get; set; }
        public string? Icon { get; set; }
        public string? ImageName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsMine { get; set; }


        public List<SubCategory> PSubCategory { get; set; }
        public List<SubCategory> SSubCategory { get; set; }
        public List<Product> Product { get; set; }
        public List<PropertyNameCategory> PropertyNameCategories { get; set; }

    }
}
