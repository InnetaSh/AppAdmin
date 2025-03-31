using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    public class Test
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public int TimeSec { get; set; }

        public String Description { get; set; }
        public override string ToString() => Title;
    }
}
