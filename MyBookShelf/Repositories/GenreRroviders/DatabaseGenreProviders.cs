using Microsoft.EntityFrameworkCore;
using MyBookShelf.DBContext;
using MyBookShelf.Models;

namespace MyBookShelf.Repositories.GenreRroviders
{
    public class DatabaseGenreProviders : IGenreProviders
    {
        private readonly BookShelfDbContextFactory _dbContextFactory;
        
        public DatabaseGenreProviders(BookShelfDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task AddAsync(Genre entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Genres.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(Genre entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Genres.Remove(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Genres.ToListAsync();
            }
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Genres.FindAsync(id);
            }
        }

        public async Task<bool> UpdateAsync(Genre entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Genres.Update(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

    }
}
