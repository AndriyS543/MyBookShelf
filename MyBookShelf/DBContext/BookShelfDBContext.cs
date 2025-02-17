using Microsoft.EntityFrameworkCore;
using MyBookShelf.DBContext.EntityConfigurations;
using MyBookShelf.Models;

namespace MyBookShelf.DBContext
{
    public class BookShelfDBContext : DbContext
    {
        public BookShelfDBContext(DbContextOptions<BookShelfDBContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ReadingSession> ReadingSessions { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new ReadingSessionConfiguration());
            modelBuilder.ApplyConfiguration(new NoteConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new ShelfConfiguration());
            modelBuilder.ApplyConfiguration(new BookGenreConfiguration());
        }
    }

}
