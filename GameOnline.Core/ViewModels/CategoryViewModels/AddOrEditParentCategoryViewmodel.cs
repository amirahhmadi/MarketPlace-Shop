using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnline.Core.ViewModels.CategoryViewModels
{
    public class AddOrEditParentCategoryViewmodel
    {
        public int SubId { get; set; }
        public List<int>? ParentId { get; set; }
    }
}
