using System.Windows.Input;
using MyBookShelf.Utilities;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.NoteProviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;


namespace MyBookShelf.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        // Dependencies for data management
        private readonly IShelfProviders _shelfProvider;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly IReadingSessionProviders _readingSessionProvider;
        private readonly INoteProviders _noteProviders;
        private readonly ICreator _creator;

        // State properties to track UI navigation status
        private bool _isBooksChecked;
        public bool IsBooksChecked
        {
            get => _isBooksChecked;
            set
            {
                _isBooksChecked = value;
                OnPropertyChanged();
            }
        }

        private bool _isShelvesChecked;
        public bool IsShelvesChecked
        {
            get => _isShelvesChecked;
            set
            {
                _isShelvesChecked = value;
                OnPropertyChanged();
            }
        }

        private bool _isReadingChecked;
        public bool IsReadingChecked
        {
            get => _isReadingChecked;
            set
            {
                _isReadingChecked = value;
                OnPropertyChanged();
            }
        }

        private bool _isInfoChecked;
        public bool IsInfoChecked
        {
            get => _isInfoChecked;
            set
            {
                _isInfoChecked = value;
                OnPropertyChanged();
            }
        }

        // Stack to store navigation history
        private readonly Stack<object> _viewHistory = new();

        // Property to manage the currently displayed view
        private object? _currentView;
        public object? CurrentView
        {
            get { return _currentView; }
            set
            {
                // Save current view in history before switching (if not going back)
                if (_currentView != null && !_isGoingBack)
                    _viewHistory.Push(_currentView);

                _currentView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanGoBack));

                // Update navigation indicators based on the selected view
                IsBooksChecked = CurrentView is BooksMainViewModel || CurrentView is SelectedBookViewModel;
                IsShelvesChecked = CurrentView is ShelvesViewModel;
                IsReadingChecked = CurrentView is ReadingMainViewModel || CurrentView is SelectedBookToReadViewModel;
                IsInfoChecked = CurrentView is InfoViewModel;
            }
        }

        private bool _isGoingBack = false; // Flag to prevent storing history when going back
        public bool CanGoBack => _viewHistory.Count > 0; // Determines if back navigation is possible

        // Commands for UI navigation
        public ICommand BooksCommand { get; set; }
        public ICommand ShelvesCommand { get; set; }

        public ICommand ReadingCommand { get; set; }

        public ICommand InfoCommand { get; set; }
        public ICommand GoBackCommand { get; }

        // Methods to handle navigation to different sections
        private void BooksMain(object obj) => CurrentView = new BooksMainViewModel(this,_creator, _shelfProvider, _bookProviders, _bookGenreProviders, _genreProviders);
        private void Shelves(object obj) => CurrentView = new ShelvesViewModel(_creator,_shelfProvider);

        private void Reading(object obj) => CurrentView = new ReadingMainViewModel(this, _creator, _shelfProvider, _bookProviders, _bookGenreProviders, _genreProviders);

        private void Info(object obj) => CurrentView = new InfoViewModel();

        // Command for navigating from BooksMain to SelectedBook
        public ICommand OpenSelectedBookCommand { get; }
        private void OpenSelectedBook(object obj)
        {
            if (obj is int IdBook)
            {
                CurrentView = new SelectedBookViewModel(IdBook,this, _bookProviders, _bookGenreProviders, _genreProviders,_noteProviders);
            }
        }

        // ReadingMain --> SelectedBookReading
        public ICommand OpenSelectedBookToReadCommand { get; }
        private void OpenSelectedBookToRead(object obj)
        {
            if (obj is int IdBook)
            {
                CurrentView = new SelectedBookToReadViewModel(IdBook,_creator,_bookProviders, this,_readingSessionProvider, _noteProviders);
            }
        }

        // Constructor initializes dependencies and sets up commands
        public NavigationViewModel(ICreator creator, IShelfProviders shelfProviders, IBookProviders bookProviders,IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders,IReadingSessionProviders readingSessionProviders,INoteProviders noteProviders)
        {
            BooksCommand = new RelayCommand(BooksMain);
            ShelvesCommand = new RelayCommand(Shelves);
            ReadingCommand = new RelayCommand(Reading);
            InfoCommand = new RelayCommand(Info);

            _shelfProvider = shelfProviders;
            _bookProviders = bookProviders;
            _genreProviders = genreProviders;
            _bookGenreProviders = bookGenreProviders;
            _readingSessionProvider = readingSessionProviders;
            _noteProviders = noteProviders;
            _creator = creator;

            // Command to navigate back
            GoBackCommand = new RelayCommand(GoBack, _ => _viewHistory.Count > 0);

            // Command for opening a selected book
            OpenSelectedBookCommand = new RelayCommand(OpenSelectedBook);

            //For ReadingMain -> selectedBookToRead
            OpenSelectedBookToReadCommand = new RelayCommand(OpenSelectedBookToRead);


            // Set initial page to BooksMainViewModel
            CurrentView = new BooksMainViewModel(this,_creator, _shelfProvider, _bookProviders, _bookGenreProviders, _genreProviders);
        }

        // Handles back navigation logic
        private void GoBack(object obj)
        {
            if (_viewHistory.Count > 0)
            {           
                _isGoingBack = true;
                CurrentView = _viewHistory.Pop();

                // Fully reload the page when navigating back to BooksMainViewModel
                if (CurrentView is BooksMainViewModel)
                {
                    CurrentView = new BooksMainViewModel(this, _creator, _shelfProvider, _bookProviders, _bookGenreProviders, _genreProviders);
                }

                _isGoingBack = false;
                OnPropertyChanged(nameof(CanGoBack));
            }
        }
    }
}
