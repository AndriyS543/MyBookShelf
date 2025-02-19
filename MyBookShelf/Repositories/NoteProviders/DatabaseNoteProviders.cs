using Microsoft.EntityFrameworkCore;
using MyBookShelf.DBContext;
using MyBookShelf.Models;

namespace MyBookShelf.Repositories.NoteProviders
{
    public class DatabaseNoteProviders : INoteProviders
    {
        private readonly BookShelfDbContextFactory _dbContextFactory;

        public DatabaseNoteProviders(BookShelfDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task AddAsync(Note entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Notes.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(Note entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Notes.Remove(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Notes.ToListAsync();
            }
        }

        public async Task<Note> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Notes.FindAsync(id);
            }
        }

        public async Task<bool> UpdateAsync(Note entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Notes.Update(entity);
                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
