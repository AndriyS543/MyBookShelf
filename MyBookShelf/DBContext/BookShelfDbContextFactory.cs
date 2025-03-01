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
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new BookShelfDBContext(options);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
