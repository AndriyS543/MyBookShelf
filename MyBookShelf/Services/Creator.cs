using MyBookShelf.Models;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Repositories.ShelfProviders;

namespace MyBookShelf.Services
{
    public class Creator : ICreator
    {
        public readonly IShelfProviders _shelfProviders;
        public readonly IBookProviders _bookProviders;
        public readonly IReadingSessionProviders _readingSessionProviders;
        public Creator(IShelfProviders shelfProviders , IBookProviders bookProviders,IReadingSessionProviders readingSessionProviders)
        {
            _shelfProviders = shelfProviders;
            _bookProviders = bookProviders;
            _readingSessionProviders = readingSessionProviders;
        }

        public async Task<Shelf> CreateShelfAsync(string nameShelf, string description)
        {
            var shelf = new Shelf()
            {
                Name = nameShelf,
                Description = description
            };
            await _shelfProviders.AddAsync(shelf);
            return shelf;
        }

        public async Task<Book> CreateBookAsync(string title, int countPages,  int shelfId ,string author = "", string description = "", string pathImg = "", int rating = 0)
        {
            var book = new Book
            {
                Title = title,
                Author = author,
                Description = description,
                CountPages = countPages,
                PublicationDate = DateTime.Now,
                PathImg = pathImg,
                Rating = rating,
                IdShelf = shelfId, 
            };
            await _bookProviders.AddAsync(book);
            return book;
        }

        public async Task<ReadingSession> CreateReadingSessionAsync(int idBook, TimeSpan readingTime,  int startPage, int finishPage, int finishPercent)
        {
            var readingSession = new ReadingSession
            {
                IdBook = idBook,
                ReadingTime = readingTime,
                StartPage = startPage,
                FinishPage = finishPage,
                FinishPercent = finishPercent
            };
            await _readingSessionProviders.AddAsync(readingSession);
            return readingSession;
        }
    }
}
