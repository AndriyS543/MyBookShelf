using MyBookShelf.Models;

namespace MyBookShelf.Repositories.NoteProviders
{
    public interface INoteProviders : IRepository<Note>
    {
        Task<IEnumerable<Note>> GetNotesByBookIdAsync(int bookId);
    }
}
