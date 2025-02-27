using System.Windows.Input;
using Learning_Words.Utilities;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.NoteProviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using MyBookShelf.View;

namespace MyBookShelf.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly IShelfProviders _shelfProvider;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly IReadingSessionProviders _readingSessionProvider;
        private readonly INoteProviders _noteProviders;
        private readonly ICreator _creator;

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

        private readonly Stack<object> _viewHistory = new();

        private object? _currentView;
        public object? CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != null && !_isGoingBack)
                    _viewHistory.Push(_currentView);

                _currentView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanGoBack));

                IsBooksChecked = CurrentView is BooksMainViewModel || CurrentView is SelectedBookViewModel;
                IsShelvesChecked = CurrentView is ShelvesViewModel;
                IsReadingChecked = CurrentView is ReadingMainViewModel || CurrentView is SelectedBookToReadViewModel;
            }
        }

        private bool _isGoingBack = false;
        public bool CanGoBack => _viewHistory.Count > 0;

        public ICommand BooksCommand { get; set; }
        public ICommand ShelvesCommand { get; set; }

        public ICommand ReadingCommand { get; set; }

        public ICommand GoBackCommand { get; } 
        private void BooksMain(object obj) => CurrentView = new BooksMainViewModel(this,_creator, _shelfProvider, _bookProviders, _bookGenreProviders, _genreProviders);
        private void Shelves(object obj) => CurrentView = new ShelvesViewModel(_creator,_shelfProvider);

        private void Reading(object obj) => CurrentView = new ReadingMainViewModel(this, _creator, _shelfProvider, _bookProviders, _bookGenreProviders, _genreProviders);

        // BooksMain --> SelectedBook 
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

        public NavigationViewModel(ICreator creator, IShelfProviders shelfProviders, IBookProviders bookProviders,IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders,IReadingSessionProviders readingSessionProviders,INoteProviders noteProviders)
        {
            BooksCommand = new RelayCommand(BooksMain);
            ShelvesCommand = new RelayCommand(Shelves);
            ReadingCommand = new RelayCommand(Reading);

            _shelfProvider = shelfProviders;
            _bookProviders = bookProviders;
            _genreProviders = genreProviders;
            _bookGenreProviders = bookGenreProviders;
            _readingSessionProvider = readingSessionProviders;
            _noteProviders = noteProviders;
            _creator = creator;

            GoBackCommand = new RelayCommand(GoBack, _ => _viewHistory.Count > 0);

            //For selectedBook
            OpenSelectedBookCommand = new RelayCommand(OpenSelectedBook);

            //For ReadingMain -> selectedBookToRead
            OpenSelectedBookToReadCommand = new RelayCommand(OpenSelectedBookToRead);


            // Startup Page
            CurrentView = new BooksMainViewModel(this,_creator, _shelfProvider, _bookProviders, _bookGenreProviders, _genreProviders);
        }

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
