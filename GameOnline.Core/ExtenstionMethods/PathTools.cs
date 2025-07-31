using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnline.Core.ExtenstionMethods
{
    public static class PathTools
    {
        public static string PathBrandImageAdmin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Brand/");
        public static string PathBrandImageClient = "/Brand/";
        
        public static string PathSliderImageAdmin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Slider/");
        public static string PathSliderImageClient = "/Slider/";
        
        public static string PathCategoryImageAdmin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Category/");
        public static string PathCategoryImageClient = "/Category/";
        
        public static string PathProductImageAdmin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Product/");
        public static string PathProductImageClient = "/Product/"; 
        
        public static string PathGalleryImageAdmin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Gallery/");
        public static string PathGalleryImageClient = "/Gallery/";
    }
}
