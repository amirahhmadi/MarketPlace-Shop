using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnline.DataBase.Entities.Sliders
{
    public class Slider : BaseEntity
    {
        public string ImageName { get; set; }
        public string? Link { get; set; }
        public int Sort { get; set; }
        public bool IsActive { get; set; }
    }
}
