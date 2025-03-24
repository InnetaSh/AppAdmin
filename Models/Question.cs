using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    internal class Question
    {
        public String Text { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
