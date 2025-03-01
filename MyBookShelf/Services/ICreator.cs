using MyBookShelf.Models;

namespace MyBookShelf.Services
{
    public interface ICreator
    {
        Task<Shelf> CreateShelfAsync(string nameShelf, string description);
        Task<Book> CreateBookAsync(string title, int countPages, int shelfId,string author, string description, string pathImg , int rating);
        Task<ReadingSession> CreateReadingSessionAsync(int idBook, TimeSpan readingTime, int startPage, int finishPage, int finishPercent);
    }
}
