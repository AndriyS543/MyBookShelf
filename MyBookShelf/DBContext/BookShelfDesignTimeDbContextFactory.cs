using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace MyBookShelf.DBContext
{
    public class BookShelfDesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookShelfDBContext>
    {
        public BookShelfDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookShelfDBContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-JLUU10E;Database=BookShelfDB6;Trusted_Connection=True;TrustServerCertificate=True;");
            return new BookShelfDBContext(optionsBuilder.Options);
        }
    }
}
