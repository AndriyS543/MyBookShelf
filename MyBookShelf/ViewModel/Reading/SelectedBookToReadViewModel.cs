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
using System.Net;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{
    public class SelectedBookToReadViewModel : ViewModelBase
    {
        // Services for data access
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly ICreator _creator;
        private readonly INoteProviders _noteProviders;
        private readonly IReadingSessionProviders _readingSessionProvider;
        private readonly int _bookID;

        private readonly NavigationViewModel _navigationViewModel;

        private Book _selectedBook = new Book();

        public ObservableCollection<ReadingSession> ReadingSessions { get; } = new ObservableCollection<ReadingSession>();

        private ReadingSession _selectedSession;
        public ReadingSession SelectedSession
        {
            get => _selectedSession;
            set  {
                if (_selectedSession != value)
                {
                    _selectedSession = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _totalFinishPercent;
        public string TotalFinishPercent
        {
            get => _totalFinishPercent;
            set
            {
                _totalFinishPercent = value;
                OnPropertyChanged(nameof(TotalFinishPercent));
            }
        }
        private string? _tbBookTitle;
        public string? tbBookTitle
        {
            get => _tbBookTitle;
            set
            {
                if (_tbBookTitle != value)
                {
                    _tbBookTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _imagePath;
        public string imagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartReadingCommand { get; }
        public ICommand DeleteSessionCommand { get; }

        public SelectedBookToReadViewModel(int id,ICreator creator, IBookProviders bookProviders ,NavigationViewModel navigationViewModel,IReadingSessionProviders readingSessionProviders, INoteProviders noteProviders)
        {
            _navigationViewModel = navigationViewModel;
            _readingSessionProvider = readingSessionProviders;
            _noteProviders = noteProviders;
            _bookProviders = bookProviders;
            _creator = creator;
            _bookID = id;
            StartReadingCommand = new RelayCommand(OpenReadingBookDialog);
            DeleteSessionCommand = new AsyncRelayCommand(DeleteRow);
            InitializeAsync(id); // Load initial data
        }

        private async void InitializeAsync(int idBook)
        {
            _selectedBook = await _bookProviders.GetByIdAsync(idBook);
            imagePath = _selectedBook.PathImg;
            var sessions = await _readingSessionProvider.GetAllByBookAsync(_selectedBook);
            var percent = sessions.Sum(s => s.FinishPercent) > 100 ? 100 : sessions.Sum(s => s.FinishPercent);
            TotalFinishPercent = percent.ToString() + "%";
            tbBookTitle = _selectedBook.Title;

            ReadingSessions.Clear();
            
            foreach (var session in sessions)
            {
                ReadingSessions.Add(new ReadingSession
                {
                    IdReadingSession = session.IdReadingSession,
                    ReadingTime = session.ReadingTime,
                    FinishPage = session.FinishPage - session.StartPage,
                    FinishPercent = session.FinishPercent,
                    IdBook = session.IdBook,
                    Book = session.Book,
                    Notes = session.Notes
                });
            }
        }

        private void OpenReadingBookDialog(object o)
        {
            var viewModel = new ReadingBookViewModel(_selectedBook,_creator,_readingSessionProvider,_noteProviders);
            var addBookWindow = new ReadingBook
            {
                DataContext = viewModel,
            };
            addBookWindow.ShowDialog();
            InitializeAsync(_bookID);
        }

        private async Task DeleteRow()
        {
            if (SelectedSession != null)
            {
                // Отримуємо всі нотатки, що належать вибраній сесії
                var notes = await _noteProviders.GetAllAsync();
                var notesToDelete = notes.Where(n => n.IdReadingSession == SelectedSession.IdReadingSession).ToList();

                // Видаляємо кожну нотатку
                foreach (var note in notesToDelete)
                {
                    await _noteProviders.DeleteAsync(note);
                }

                // Видаляємо саму сесію
                await _readingSessionProvider.DeleteAsync(SelectedSession);

                // Очищаємо вибрану сесію
                SelectedSession = null;
                InitializeAsync(_bookID);
            }
        }

    }
}
