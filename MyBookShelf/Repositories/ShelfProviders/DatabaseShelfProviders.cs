using Microsoft.EntityFrameworkCore;
using MyBookShelf.DBContext;
using MyBookShelf.Models;

namespace MyBookShelf.Repositories.ShelfProviders
{
    public class DatabaseShelfProviders : IShelfProviders
    {
        private readonly BookShelfDbContextFactory _dbContextFactory;

        public DatabaseShelfProviders(BookShelfDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task AddAsync(Shelf entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Shelves.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(Shelf entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Shelves.Remove(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<IEnumerable<Shelf>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Shelves.ToListAsync();
            }
        }

        public async Task<Shelf> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Shelves.FindAsync(id);
            }
        }

        public async Task<bool> UpdateAsync(Shelf entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Shelves.Update(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
