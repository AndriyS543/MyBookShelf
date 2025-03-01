using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using MyBookShelf.View;
using MyBookShelf.Utilities;
namespace MyBookShelf.ViewModel
{
    public class BooksMainViewModel : ViewModelBase
    {
        // Services for data access
        private readonly IShelfProviders _shelfProvider;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly ICreator _creator;
        private readonly NavigationViewModel _navigationViewModel;

        // Collections for UI binding
        public ObservableCollection<Shelf> Shelves { get; } = new ObservableCollection<Shelf>();
        public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();
        public ObservableCollection<Book> FilteredBooks { get; } = new ObservableCollection<Book>();

        private Shelf _selectedShelf = new Shelf();
        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                if (_selectedShelf != value)
                {
                    _selectedShelf = value;
                    OnPropertyChanged();
                    LoadBooks(); // Load books when shelf selection changes
                }
            }
        }

        public ICommand TemplateClickCommand { get; }

        public BooksMainViewModel(
            NavigationViewModel navigationViewModel,
            ICreator creator,
            IShelfProviders shelfProviders,
            IBookProviders bookProviders,
            IBookGenreProviders bookGenreProviders,
            IGenreProviders genreProviders)
        {
            _creator = creator;
            _shelfProvider = shelfProviders;
            _bookProviders = bookProviders;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;
            _navigationViewModel = navigationViewModel;

            TemplateClickCommand = new RelayCommand(OnBookClicked);

            InitializeAsync(); // Load initial data
        }

        /// <summary>
        /// Asynchronously loads shelves and books from providers.
        /// </summary>
        private async void InitializeAsync()
        {
            try
            {
                Shelves.Clear();
                var shelves = await _shelfProvider.GetAllAsync();
                var allBooks = shelves.SelectMany(s => s.Books).ToList();

                Shelves.Add(new Shelf { Name = "My Book Shelf", IdShelf = -1, Books = allBooks });
                if (shelves.Any())
                {

                    foreach (var shelf in shelves)
                    {
                        Shelves.Add(shelf);
                    }
                }

                SelectedShelf = Shelves.FirstOrDefault() ?? new Shelf { Name = "Default Shelf", IdShelf = -2, Books = new List<Book>() };

                Books.Clear();
                var books = await _bookProviders.GetAllAsync();
                foreach (var book in books)
                {
                    Books.Add(book);
                }

                FilterBooks();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                // Log or handle exception if needed
            }
        }

        /// <summary>
        /// Updates the filtered book collection based on the selected shelf.
        /// </summary>
        private void LoadBooks()
        {
            FilterBooks();
        }

        /// <summary>
        /// Filters books based on the currently selected shelf.
        /// </summary>
        private void FilterBooks()
        {
            try
            {
                FilteredBooks.Clear();

                if (SelectedShelf == null) return; // Prevent null reference exception

                if (SelectedShelf.IdShelf == -1)
                {
                    // Show all books if "My Book Shelf" is selected
                    foreach (var book in Books)
                    {
                        FilteredBooks.Add(book);
                    }
                }
                else
                {
                    // Show only books belonging to the selected shelf
                    foreach (var book in Books.Where(b => b.IdShelf == SelectedShelf.IdShelf))
                    {
                        FilteredBooks.Add(book);
                    }
                    // Add a placeholder for adding new books
                    FilteredBooks.Add(new Book { Title = "+", IdShelf = SelectedShelf.IdShelf });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                // Log or handle exception if needed
            }
        }

        /// <summary>
        /// Handles book click events.
        /// </summary>
        private void OnBookClicked(object param)
        {
            try
            {
                if (param is Book book)
                {
                    if (book.IdShelf == -1 || book.Title == "+")
                    {
                        OpenAddNewBookDialog();
                    }
                    else
                    {
                        // Navigate to the selected book
                        _navigationViewModel.OpenSelectedBookCommand.Execute(book.IdBook);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                // Log or handle exception if needed
            }
        }

        /// <summary>
        /// Opens the dialog for adding a new book.
        /// </summary>
        private void OpenAddNewBookDialog()
        {
            var viewModel = new AddNewBookViewModel(SelectedShelf, _creator, _bookGenreProviders, _genreProviders);
            var addBookWindow = new AddNewBook
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow
            };
            addBookWindow.ShowDialog();

            InitializeAsync(); // Refresh book list
        }
    }
}