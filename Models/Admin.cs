using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    internal class Admin
    {
        public String Name { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();

        public void SetSelectedCategory(int index)
        {
            Categories.ForEach(x => x.IsSelected = false);
            Categories[index].IsSelected = true;
        }
    }
}
