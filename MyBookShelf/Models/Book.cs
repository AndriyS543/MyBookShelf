namespace MyBookShelf.Models
{
    public class Book
    {
        public int IdBook { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PathImg { get; set; }
        public int IdShelf { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int CountPages { get; set; }

        public Shelf Shelf { get; set; }

        public List<ReadingSession> ReadingSessions { get; set; }

        public List<BookGenre> BookGenres { get; set; }
    }
}
