using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAdmin.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public String AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
