using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnline.Core.ViewModels.BrandViewModels
{
    public class CreateBrandsViewModel
    {
        public IFormFile? ImageName { get; set; }
        public string FaTitle { get; set; }
        public string EnTitle { get; set; }
        public string? Description { get; set; }
    }
}
