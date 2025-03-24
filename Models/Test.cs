using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    internal class Test
    {
        public String Title { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();

        public override string ToString() => Title;
    }
}
