using MyBookShelf.Models;

namespace MyBookShelf.Repositories.ReadingSessionProviders
{
    public interface IReadingSessionProviders : IRepository<ReadingSession>
    {
        Task<List<ReadingSession>> GetAllByBookAsync(Book book);
    }
}
