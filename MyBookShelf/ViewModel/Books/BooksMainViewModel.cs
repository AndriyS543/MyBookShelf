using Learning_Words.Utilities;
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
namespace MyBookShelf.ViewModel
{
    public class BooksMainViewModel : ViewModelBase
    {
        private readonly IShelfProviders _shelfProvider;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly ICreator _creator;
        private readonly NavigationViewModel _navigationViewModel;


        public ObservableCollection<Shelf> Shelves { get; set; } = new ObservableCollection<Shelf>();
        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        public ObservableCollection<Book> FilteredBooks { get; set; } = new ObservableCollection<Book>();

        private Shelf _selectedShelf;
        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                if (_selectedShelf != value)
                {
                    _selectedShelf = value;
                    OnPropertyChanged();
                    if (SelectedShelf != null)
                    {
                        LoadBooks();
                    }
                }
            }
        }

        public ICommand TemplateClickCommand { get; set; }

        public BooksMainViewModel(NavigationViewModel navigationViewModel, ICreator creator, IShelfProviders shelfProviders, IBookProviders bookProviders, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _creator = creator;
            _shelfProvider = shelfProviders;
            _bookProviders = bookProviders;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;

            TemplateClickCommand = new RelayCommand(param => OnBookClicked(param));

            _navigationViewModel = navigationViewModel;
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            Shelves.Clear();


            var shelves = await _shelfProvider.GetAllAsync();
            var allBooks = shelves.SelectMany(s => s.Books).ToList();
            Shelves.Add(new Shelf { Name = "My Book Shelf", IdShelf = -1, Books = allBooks });
            foreach (var shelf in shelves)
            {
                Shelves.Add(shelf);
            }
            SelectedShelf = Shelves[0];

            Books.Clear();
            var books = await _bookProviders.GetAllAsync();
            foreach (var book in books)
            {
                Books.Add(book);
            }
            FilterBooks();
        }



        private void LoadBooks()
        {
            FilterBooks();
        }

        private void FilterBooks()
        {
            try
            {
                FilteredBooks.Clear();

                if (SelectedShelf?.IdShelf == -1)
                {
                    foreach (var book in Books)
                    {
                        FilteredBooks.Add(book);
                    }
                }
                else
                {
                    foreach (var book in Books.Where(b => b.IdShelf == SelectedShelf.IdShelf))
                    {
                        FilteredBooks.Add(book);
                    }
                    FilteredBooks.Add(new Book { Title = "+", IdShelf = SelectedShelf.IdShelf });
                }
            }
            catch (Exception ex) { }
        }

        private void OnBookClicked(object param)
        {
            try
            {
                if (param is Book book)
                {
                    if (book.IdShelf == -1 || book.Title == "+")
                    {
                        var viewModel = new AddNewBookViewModel(SelectedShelf, _creator, _bookProviders, _bookGenreProviders, _genreProviders);
                        var addBookWindow = new AddNewBook
                        {
                            DataContext = viewModel,
                            Owner = Application.Current.MainWindow
                        };
                        addBookWindow.ShowDialog();
                        InitializeAsync();

 
                    }
                    else
                    {
                        _navigationViewModel.OpenSelectedBookCommand.Execute(book.IdBook);
                    }
                }
            }
            catch (Exception ex) { }
        }


    }

}