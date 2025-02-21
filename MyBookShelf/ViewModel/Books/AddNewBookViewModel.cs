using Learning_Words.Utilities;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using System.Windows;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{


    public class AddNewBookViewModel : Window
    {
        private string _tbCountPage;
        public string tbCountPage
        {
            get => _tbCountPage;
            set
            {
                if (_tbCountPage != value)
                {
                    _tbCountPage = value;
                }
            }
        }

        private string _tbBookAuthor;
        public string tbBookAuthor
        {
            get => _tbBookAuthor;
            set
            {
                if (_tbBookAuthor != value)
                {
                    _tbBookAuthor = value;
                }
            }
        }

        private string _tbBookTitle;
        public string tbBookTitle
        {
            get => _tbBookTitle;
            set
            {
                if (_tbBookTitle != value)
                {
                    _tbBookTitle = value;

                }
            }
        }
        private readonly Shelf _shelf;
        private readonly IShelfProviders _shelfProvider;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly ICreator _creator;
        public ICommand AddBooksCommand { get; }

        public AddNewBookViewModel(Shelf shelf,ICreator creator,IBookProviders bookProviders,IBookGenreProviders bookGenreProviders,IGenreProviders genreProviders)
        {
            _shelf = shelf;
            _creator = creator;
            _bookProviders = bookProviders;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;

            AddBooksCommand = new AsyncRelayCommand(AddBooksAsync);
        }
        private async Task AddBooksAsync() 
        {
            await _creator.CreateBookAsync(tbBookTitle,
                Int32.Parse(_tbCountPage),
                _shelf.IdShelf,
                tbBookAuthor,
                "Default Description",
                "Default PathImg",
                 0);

        }

    }
}
