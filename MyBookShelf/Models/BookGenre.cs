using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShelf.Models
{
    public class BookGenre
    {
        public int IdBook { get; set; }
        public int IdGenre { get; set; }

        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }

}
