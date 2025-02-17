using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShelf.Models
{
    public class Book
    {
        public int IdBook { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PathImg { get; set; }
        public int IdShelf { get; set; }
        public decimal Rating { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int CountPages { get; set; }

        public Shelf Shelf { get; set; }
    }
}
