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
using Microsoft.Win32;
using MyBookShelf.Utilities;
using System.IO;
namespace MyBookShelf.ViewModel
{
    public class SelectedBookViewModel : ViewModelBase
    {
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly NavigationViewModel _navigationViewModel;

        private Book _selectedBook;
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Author));
                OnPropertyChanged(nameof(PageCountText));
                OnPropertyChanged(nameof(SelectedImagePath));
                OnPropertyChanged(nameof(ShelfDescription));
                CheckBookContentChanged();
            }
        }

        private string _title;
        private string _author;
        private string _genre;
        private int? _pageCount;
        private string _selectedImagePath;
        private string _shelfDescription;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); CheckBookContentChanged(); }
        }

        public string Author
        {
            get => _author;
            set { _author = value; OnPropertyChanged(); CheckBookContentChanged(); }
        }

        public string Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(); CheckBookContentChanged(); }
        }

        public string PageCountText
        {
            get => _pageCount?.ToString() ?? string.Empty;
            set
            {
                if (int.TryParse(value, out int result))
                    _pageCount = result;
                else
                    _pageCount = null;

                OnPropertyChanged();
                CheckBookContentChanged();
            }
        }

        public string SelectedImagePath
        {
            get => _selectedImagePath;
            set { _selectedImagePath = value; OnPropertyChanged(); CheckBookContentChanged(); }
        }

        public string ShelfDescription
        {
            get => _shelfDescription;
            set { _shelfDescription = value; OnPropertyChanged(); CheckBookContentChanged(); }
        }

        private bool _isBookContentChanged;
        public bool IsBookContentChanged
        {
            get => _isBookContentChanged;
            set
            {
                _isBookContentChanged = value;
                OnPropertyChanged();
            }
        }
        private bool _isDeleteBookButton = true;
        public bool IsDeleteBookButton
        {
            get => _isDeleteBookButton;
            set { _isDeleteBookButton = value; OnPropertyChanged(); }
        }

        private bool _isDeleteBook = false;
        public bool IsDeleteBook
        {
            get => _isDeleteBook;
            set { _isDeleteBook = value; OnPropertyChanged(); }
        }

        public ICommand DeleteBookCommand { get; }
        public ICommand YesDeleteBookCommand { get; }
        public ICommand NoDeleteBookCommand { get; }
        public ICommand CommitChangesBookCommand { get; }
        public ICommand CancelChangesBookCommand { get; }

        public ICommand SelectFb2FileCommand { get; }

        public SelectedBookViewModel(int bookId, NavigationViewModel navigationViewModel, IBookProviders bookProviders, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _bookProviders = bookProviders;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;
            _navigationViewModel = navigationViewModel;

            DeleteBookCommand = new RelayCommand(DeleteBook);
            YesDeleteBookCommand = new AsyncRelayCommand(ConfirmDeleteBookAsync);
            NoDeleteBookCommand = new RelayCommand(CancelDeleteBook);
            CommitChangesBookCommand = new AsyncRelayCommand(CommitBookChangesAsync);
            CancelChangesBookCommand = new RelayCommand(CancelBookChanges);
            SelectFb2FileCommand = new AsyncRelayCommand(SelectFb2File);

            LoadBookData(bookId);
        }

        private async void LoadBookData(int bookId)
        {
            SelectedBook = await _bookProviders.GetByIdAsync(bookId);
            if (SelectedBook != null)
            {
                Title = SelectedBook.Title;
                Author = SelectedBook.Author;
                PageCountText = SelectedBook.CountPages.ToString();
                SelectedImagePath = SelectedBook.PathImg;
                ShelfDescription = SelectedBook.Description;
            }
        }

        private void DeleteBook(object obj)
        {
            IsDeleteBookButton = false;
            IsDeleteBook = true;
        }

        private void CancelDeleteBook(object obj)
        {
            IsDeleteBookButton = true;
            IsDeleteBook = false;
        }

        private async Task ConfirmDeleteBookAsync()
        {
            if (SelectedBook == null) return;

            if (_navigationViewModel.GoBackCommand.CanExecute(null))
            {
                _navigationViewModel.GoBackCommand.Execute(null);
            }

            await _bookProviders.DeleteAsync(SelectedBook);

            IsDeleteBookButton = true;
            IsDeleteBook = false;
        }

        private void CancelBookChanges(object obj)
        {
            if (SelectedBook != null)
            {
                Title = SelectedBook.Title;
                Author = SelectedBook.Author;
                PageCountText = SelectedBook.CountPages.ToString();
                SelectedImagePath = SelectedBook.PathImg;
                ShelfDescription = SelectedBook.Description;
            }
        }

        private async Task CommitBookChangesAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title) && SelectedBook != null)
            {
                var updatedBook = new Book { IdBook = SelectedBook.IdBook, Title = Title, Author = Author, CountPages = _pageCount ?? 0, PathImg = SelectedImagePath, Description = ShelfDescription, IdShelf = SelectedBook.IdShelf };
                await _bookProviders.UpdateAsync(updatedBook);
                SelectedBook = updatedBook;
            }
        }

        private void CheckBookContentChanged()
        {
            if (SelectedBook == null) return;

            IsBookContentChanged =
                Title != SelectedBook.Title ||
                Author != SelectedBook.Author ||
                PageCountText != SelectedBook.CountPages.ToString() ||
                SelectedImagePath != SelectedBook.PathImg ||
                ShelfDescription != SelectedBook.Description;

            OnPropertyChanged(nameof(IsBookContentChanged));
        }

        private async Task SelectFb2File()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "FB2 Files|*.fb2|Image Files|*.png;*.jpg",
                Title = "Select a File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(openFileDialog.FileName).ToLower();
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                if (extension == ".fb2")
                {
                    var result = await Fb2Parser.ParseFb2FileAsync(openFileDialog.FileName);
                    Title = result.bookTitle;
                    Author = result.bookAuthor;
                    ShelfDescription = result.bookDescription;
                    SelectedImagePath = result.imagePath;
                }
                else if (extension == ".png" || extension == ".jpg")
                {
                    string newFileName = Guid.NewGuid().ToString() + extension;
                    string destinationPath = Path.Combine(imagesFolder, newFileName);
                    File.Copy(openFileDialog.FileName, destinationPath, true);
                    SelectedImagePath = destinationPath;
                }
            }
        }

    }
}
