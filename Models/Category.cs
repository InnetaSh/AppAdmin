using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    internal class Category
    {
        public String Title { get; set; }
        public Boolean IsSelected { get; set; }
        public List<Test> Tests { get; set; } = new List<Test>();
    }
}
