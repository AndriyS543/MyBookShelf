namespace MyBookShelf.Models
{
    public class ReadingSession
    {
        public int IdReadingSession { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int StartPage { get; set; }
        public int FinishPage { get; set; }
        public int FinishPercent { get; set; }
        public int IdBook { get; set; }
        public Book Book { get; set; }
        public List<Note> Notes { get; set; }
    }

}
