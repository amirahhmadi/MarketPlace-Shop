using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnline.Core.ViewModels.SliderViewModels.Admin
{
    public class GetSlidersViewModel
    {
        public int SliderId { get; set; }
        public string ImageName { get; set; }
        public string? Link { get; set; }
        public bool IsActive { get; set; }
    }
}
