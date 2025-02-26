using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookShelf.Models
{
    public class Genre
    {
        public int IdGenre { get; set; }
        public string Name { get; set; }
        public List<BookGenre> BookGenres { get; set; }

    }

}
