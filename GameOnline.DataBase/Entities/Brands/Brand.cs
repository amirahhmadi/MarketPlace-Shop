using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOnline.DataBase.Entities.Products;

namespace GameOnline.DataBase.Entities.Brands
{
    public class Brand : BaseEntity
    {
        public string? ImageName { get; set; }
        public string FaTitle { get; set; }
        public string EnTitle { get; set; }
        public string? Description { get; set; }

        public List<Product> Products { get; set; }
    }
}
