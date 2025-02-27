using Learning_Words.Utilities;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.NoteProviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using MyBookShelf.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{
    public class SelectedBookToReadViewModel : ViewModelBase
    {
        // Services for data access
        private readonly IShelfProviders _shelfProvider;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly ICreator _creator;
        private readonly INoteProviders _noteProviders;
        private readonly IReadingSessionProviders _readingSessionProvider;
        private readonly NavigationViewModel _navigationViewModel;


        public ObservableCollection<ReadingSession> ReadingSession { get; } = new ObservableCollection<ReadingSession>();
        public SelectedBookToReadViewModel(
            int id,
            NavigationViewModel navigationViewModel,
            IReadingSessionProviders readingSessionProviders,
            INoteProviders noteProviders)
        {
            _navigationViewModel = navigationViewModel;
            _readingSessionProvider = readingSessionProviders;
            _noteProviders = noteProviders;

            InitializeAsync(id); // Load initial data
        }

        private async void InitializeAsync(int idBook)
        {
            var sessions = await _readingSessionProvider.GetAllByBookIdAsync(idBook);

            ReadingSession.Clear();
            foreach (var session in sessions)
            {
                ReadingSession.Add(session);
            }
        }


    }
}
