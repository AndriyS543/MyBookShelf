using MyBookShelf.Utilities;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.NoteProviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Services;
using MyBookShelf.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{
    public class SelectedBookToReadViewModel : ViewModelBase
    {
        // Services for data access
        private readonly IBookProviders _bookProviders;
        private readonly ICreator _creator;
        private readonly INoteProviders _noteProviders;
        private readonly IReadingSessionProviders _readingSessionProvider;
        private readonly int _bookID;

        private readonly NavigationViewModel _navigationViewModel;

        private Book _selectedBook = new Book();
        public ObservableCollection<ReadingSession> ReadingSessions { get; } = new ObservableCollection<ReadingSession>();

        // Property for selected reading session
        private ReadingSession? _selectedSession = new ReadingSession();
        public ReadingSession? SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (_selectedSession != value)
                {
                    _selectedSession = value;
                    OnPropertyChanged(); // Notify property change
                }
            }
        }

        // Property for total finish percentage
        private string _totalFinishPercent = "";
        public string TotalFinishPercent
        {
            get => _totalFinishPercent;
            set
            {
                _totalFinishPercent = value;
                OnPropertyChanged(nameof(TotalFinishPercent)); // Notify property change
            }
        }

        // Property for book title
        private string? _tbBookTitle;
        public string? tbBookTitle
        {
            get => _tbBookTitle;
            set
            {
                if (_tbBookTitle != value)
                {
                    _tbBookTitle = value;
                    OnPropertyChanged(); // Notify property change
                }
            }
        }

        // Property for image path
        private string _imagePath = "";
        public string imagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(); // Notify property change
            }
        }

        // Command to start reading the book
        public ICommand StartReadingCommand { get; }

        // Command to delete a reading session
        public ICommand DeleteSessionCommand { get; }

        // Constructor for initializing the ViewModel
        public SelectedBookToReadViewModel(int id, ICreator creator, IBookProviders bookProviders, NavigationViewModel navigationViewModel, IReadingSessionProviders readingSessionProviders, INoteProviders noteProviders)
        {
            _navigationViewModel = navigationViewModel;
            _readingSessionProvider = readingSessionProviders;
            _noteProviders = noteProviders;
            _bookProviders = bookProviders;
            _creator = creator;
            _bookID = id;

            // Initialize commands
            StartReadingCommand = new RelayCommand(OpenReadingBookDialog);
            DeleteSessionCommand = new AsyncRelayCommand(DeleteRow);

            // Load initial data
            InitializeAsync(id);
        }

        // Asynchronous method to initialize data for the selected book
        private async void InitializeAsync(int idBook)
        {
            // Fetch the selected book
            _selectedBook = await _bookProviders.GetByIdAsync(idBook);
            imagePath = _selectedBook.PathImg; // Set the book image path

            // Fetch reading sessions for the book
            var sessions = await _readingSessionProvider.GetAllByBookAsync(_selectedBook);

            // Calculate total reading progress (percentage)
            var percent = sessions.Sum(s => s.FinishPercent) > 100 ? 100 : sessions.Sum(s => s.FinishPercent);
            TotalFinishPercent = percent.ToString() + "%"; // Display total progress percentage
            tbBookTitle = _selectedBook.Title; // Set book title

            // Clear existing sessions and add new ones
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

        // Method to open the reading book dialog
        private void OpenReadingBookDialog(object o)
        {
            var viewModel = new ReadingBookViewModel(_selectedBook, _creator, _readingSessionProvider, _noteProviders);
            var addBookWindow = new ReadingBook
            {
                DataContext = viewModel, // Set the DataContext for binding
            };
            addBookWindow.ShowDialog(); // Show dialog for reading book
            InitializeAsync(_bookID); // Reload data after dialog
        }

        // Asynchronous method to delete the selected reading session
        private async Task DeleteRow()
        {
            if (SelectedSession != null)
            {
                // Fetch all notes and filter those associated with the selected session
                var notes = await _noteProviders.GetAllAsync();
                var notesToDelete = notes.Where(n => n.IdReadingSession == SelectedSession.IdReadingSession).ToList();

                // Delete the associated notes
                foreach (var note in notesToDelete)
                {
                    await _noteProviders.DeleteAsync(note);
                }
                // Delete the selected reading session
                await _readingSessionProvider.DeleteAsync(SelectedSession);

                // Clear the selected session
                SelectedSession = null;

                // Reload the data after deletion
                InitializeAsync(_bookID);
            }
        }
    }
}
