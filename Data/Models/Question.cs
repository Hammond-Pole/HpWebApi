using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Selection Selection { get; set; }
    }
}
