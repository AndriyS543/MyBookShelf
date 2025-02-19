using Microsoft.EntityFrameworkCore;
using MyBookShelf.DBContext;
using MyBookShelf.Models;

namespace MyBookShelf.Repositories.BookRroviders
{
    public class DatabaseBookProviders : IBookProviders
    {
        private readonly BookShelfDbContextFactory _dbContextFactory;

        public DatabaseBookProviders(BookShelfDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Book entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Books.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(Book entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Books.Remove(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Books.ToListAsync();
            }
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Books.FindAsync(id);
            }
        }

        public async Task<bool> UpdateAsync(Book entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Books.Update(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
