using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnline.DataBase.Entities.Colors
{
    public class Color : BaseEntity
    {
        public string ColorName { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
