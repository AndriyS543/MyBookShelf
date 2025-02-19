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
