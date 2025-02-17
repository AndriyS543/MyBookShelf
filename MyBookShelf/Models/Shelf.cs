using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookShelf.Models
{
    public class Shelf
    {
        public int IdShelf { get; set; }  // Primary Key
        public string Name { get; set; }
        public string Description { get; set; }

        // Навігаційна властивість для зв'язку з Book
        public List<Book> Books { get; set; }
    }

}
