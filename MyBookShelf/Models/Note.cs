using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShelf.Models
{
    public class Note
    {
        public int IdNote { get; set; }
        public int IdReadingSession { get; set; }
        public string Text { get; set; }
        public ReadingSession ReadingSession { get; set; }
    }

}
