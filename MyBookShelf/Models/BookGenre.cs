namespace MyBookShelf.Models
{
    public class BookGenre
    {
        public int IdBookGenre { get; set; }
        public int IdBook { get; set; }
        public int IdGenre { get; set; }
        public Book Book { get; set; }
        public Genre Genre { get; set; }
    }

}
