namespace MyBookShelf.Models
{
    public class Shelf
    {
        public int IdShelf { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Book> Books { get; set; }
    }

}
