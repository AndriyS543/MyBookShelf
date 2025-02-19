using Microsoft.EntityFrameworkCore;
using MyBookShelf.DBContext;
using MyBookShelf.Models;

namespace MyBookShelf.Repositories.BookGenreRroviders
{
    public class DatabaseBookGenreProviders : IBookGenreProviders
    {
        private readonly BookShelfDbContextFactory _dbContextFactory;

        public DatabaseBookGenreProviders(BookShelfDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task AddAsync(BookGenre entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.BookGenres.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(BookGenre entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.BookGenres.Remove(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<IEnumerable<BookGenre>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.BookGenres.ToListAsync();
            }
        }

        public async Task<BookGenre> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.BookGenres.FindAsync(id);
            }
        }

        public async Task<bool> UpdateAsync(BookGenre entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.BookGenres.Update(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

    }
}
