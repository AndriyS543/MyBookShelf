using Microsoft.EntityFrameworkCore;
using MyBookShelf.DBContext;
using MyBookShelf.Models;

namespace MyBookShelf.Repositories.ReadingSessionProviders
{
    public class DatabaseReadingSessionProviders : IReadingSessionProviders
    {
        private readonly BookShelfDbContextFactory _dbContextFactory;

        public DatabaseReadingSessionProviders(BookShelfDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task AddAsync(ReadingSession entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.ReadingSessions.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(ReadingSession entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.ReadingSessions.Remove(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<IEnumerable<ReadingSession>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.ReadingSessions.ToListAsync();
            }
        }

        public async Task<ReadingSession> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.ReadingSessions.FindAsync(id);
            }
        }


        public async Task<List<ReadingSession>> GetAllByBookAsync(Book book)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.ReadingSessions
                    .Where(r => r.Book == book)
                    .ToListAsync();
            }
        }

        public async Task<bool> UpdateAsync(ReadingSession entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.ReadingSessions.Update(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
