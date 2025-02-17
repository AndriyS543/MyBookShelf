using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace MyBookShelf.DBContext
{
    public class BookShelfDesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookShelfDBContext>
    {
        public BookShelfDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookShelfDBContext>();
            optionsBuilder.UseSqlServer("connection");
            return new BookShelfDBContext(optionsBuilder.Options);
        }
    }
}
