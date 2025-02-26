using Learning_Words.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using Microsoft.Win32;
using MyBookShelf.Utilities;
using System.IO;
using MyBookShelf.View;
namespace MyBookShelf.ViewModel
{
    public class SelectedBookViewModel : ViewModelBase
    {
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly int _bookID;

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
                OnPropertyChanged(nameof(BookDescription));
                CheckBookContentChanged();
            }
        }

        private string _title;
        private string _author;
        private string _genre;
        private int? _pageCount;
        private string _selectedImagePath;
        private string _bookDescription;

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

        public string BookDescription
        {
            get => _bookDescription;
            set { _bookDescription = value; OnPropertyChanged(); CheckBookContentChanged(); }
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

        private int _selectedRating = -1;
        public int SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    UpdateRatings(); 
                    CheckBookContentChanged();
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<RatingItem> Ratings { get; } = new();

        public ICommand DeleteBookCommand { get; }
        public ICommand YesDeleteBookCommand { get; }
        public ICommand NoDeleteBookCommand { get; }
        public ICommand CommitChangesBookCommand { get; }
        public ICommand CancelChangesBookCommand { get; }

        public ICommand SelectFb2FileCommand { get; }
        public ICommand SetRatingCommand { get; }
        public ICommand AddGenreCommand { get; }

        public SelectedBookViewModel(int bookId, NavigationViewModel navigationViewModel, IBookProviders bookProviders, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _bookProviders = bookProviders;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;
            _navigationViewModel = navigationViewModel;
            _bookID = bookId;
            DeleteBookCommand = new RelayCommand(DeleteBook);
            YesDeleteBookCommand = new AsyncRelayCommand(ConfirmDeleteBookAsync);
            NoDeleteBookCommand = new RelayCommand(CancelDeleteBook);
            CommitChangesBookCommand = new AsyncRelayCommand(CommitBookChangesAsync);
            CancelChangesBookCommand = new RelayCommand(CancelBookChanges);
            SelectFb2FileCommand = new AsyncRelayCommand(SelectFb2File);
            SetRatingCommand = new RelayCommand(SetRating);
            AddGenreCommand = new AsyncRelayCommand(AddGenre);

            LoadBookData(_bookID);
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
                BookDescription = SelectedBook.Description;
                SelectedRating = SelectedBook.Rating ;

                LoadBookGenresAsync();
            }
        }
        private async Task AddGenre()
        {
            var viewModel = new ManageBookGenreViewModel(SelectedBook.IdBook, _bookGenreProviders, _genreProviders);
            var addBookWindow = new ManageBookGenre
            {
                DataContext = viewModel
            };
            addBookWindow.ShowDialog();
            SelectedBook = await _bookProviders.GetByIdAsync(_bookID);
            LoadBookGenresAsync();
        }


        private async void LoadBookGenresAsync()
        {
            var allGenres = await _genreProviders.GetAllAsync(); // Отримуємо всі жанри з бази

            var bookGenres = allGenres.Where(g => SelectedBook?.BookGenres?.Any(bg => bg.IdGenre == g.IdGenre) == true);

            if (bookGenres.Any())
            {
                var genreNames = bookGenres.Select(g => g.Name).ToList();
                Genre = string.Join(", ", genreNames); // Перетворюємо у рядок через кому
            }
            else
            {
                Genre = "Без жанру"; // Якщо жанри відсутні
            }
            OnPropertyChanged(nameof(Genre)); // Оновлюємо UI
        }

        private void UpdateRatings()
        {
            Ratings.Clear();
            for (int i = 1; i <= 5; i++)
            {
                Ratings.Add(new RatingItem
                {
                    Number = i,
                    Value = i <= SelectedRating ? "★" : "☆"
                });
            }
        }
        private void SetRating(object rating)
        {
            if (rating is int newRating)
            {
                SelectedRating = newRating;
                UpdateRatings();
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
            await _bookProviders.DeleteAsync(SelectedBook);

            if (_navigationViewModel.GoBackCommand.CanExecute(null))
            {
                _navigationViewModel.GoBackCommand.Execute(null);
            }

           

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
                BookDescription = SelectedBook.Description;
                SelectedRating = SelectedBook.Rating;
            }
        }

        private async Task CommitBookChangesAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title) && SelectedBook != null)
            {
                var updatedBook = new Book { IdBook = SelectedBook.IdBook, Title = Title, Author = Author, CountPages = _pageCount ?? 0, PathImg = SelectedImagePath, Description = BookDescription,Rating= SelectedRating, IdShelf = SelectedBook.IdShelf };
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
                BookDescription != SelectedBook.Description||
                SelectedRating != SelectedBook.Rating;

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
                    BookDescription = result.bookDescription?.Length > 700
                        ? result.bookDescription.Substring(0, 700)
                        : result.bookDescription;
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
