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
    public class SelectedBookViewModel {

        private int _bookId;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        public SelectedBookViewModel(int bookid, IBookProviders bookProviders, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _bookId = bookid;
            _bookProviders = bookProviders;
            _genreProviders = genreProviders;
            _bookGenreProviders = bookGenreProviders;
        }
    }
}