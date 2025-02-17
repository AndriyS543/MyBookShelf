using Microsoft.EntityFrameworkCore;

namespace MyBookShelf.DBContext
{
    public class BookShelfDbContextFactory
    {
        private readonly string _connectionString;

        public BookShelfDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public BookShelfDBContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<BookShelfDBContext>()
                .UseSqlServer(_connectionString)
                .Options;

            return new BookShelfDBContext(options);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
