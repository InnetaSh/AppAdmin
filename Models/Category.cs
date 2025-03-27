using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    public class Category
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public Boolean IsSelected { get; set; }
        public List<Test> Tests { get; set; } = new List<Test>();

        public Category() { }
        public Category(string title)
        {
            Title = title;
        }
        public Category(string title, bool select) 
        {
            Title = title;
            IsSelected = select;
        }
    }
}
