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
        // Dependencies for data retrieval
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly int _bookID;

        // The currently selected book
        private Book _selectedBook = new Book();
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

        // Book properties for UI binding
        private string _title = "";
        private string _author = "";
        private string _genre = "";
        private int? _pageCount;
        private string _selectedImagePath = "";
        private string _bookDescription = "";

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

        // Flags to track book changes and deletion
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

        // Rating properties
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

        // Commands for user interactions
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

            // Initialize commands
            DeleteBookCommand = new RelayCommand(DeleteBook);
            YesDeleteBookCommand = new AsyncRelayCommand(ConfirmDeleteBookAsync);
            NoDeleteBookCommand = new RelayCommand(CancelDeleteBook);
            CommitChangesBookCommand = new AsyncRelayCommand(CommitBookChangesAsync);
            CancelChangesBookCommand = new RelayCommand(CancelBookChanges);
            SelectFb2FileCommand = new RelayCommand(SelectFb2File);
            SetRatingCommand = new RelayCommand(SetRating);
            AddGenreCommand = new AsyncRelayCommand(AddGenre);

            LoadBookData(_bookID);
        }

        /// <summary>
        /// Loads book data asynchronously.
        /// </summary>
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

        /// <summary>
        /// Add genres
        /// </summary>
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

        /// <summary>
        /// Loads genres associated with the selected book.
        /// </summary>
        private async void LoadBookGenresAsync()
        {
            var allGenres = await _genreProviders.GetAllAsync();

            var bookGenres = allGenres.Where(g => SelectedBook?.BookGenres?.Any(bg => bg.IdGenre == g.IdGenre) == true);

            if (bookGenres.Any())
            {
                var genreNames = bookGenres.Select(g => g.Name).ToList();
                Genre = string.Join(", ", genreNames);
            }
            else
            {
                Genre = "No genre";
            }
        }

        /// <summary>
        /// Updates the book rating UI.
        /// </summary>
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

        /// <summary>
        /// Sets the selected book rating and updates ratings
        /// </summary>
        private void SetRating(object rating)
        {
            if (rating is int newRating)
            {
                SelectedRating = newRating;
                UpdateRatings();
            }
        }

        /// <summary>
        /// Controls the visibility of the delete book button and confirmation message
        /// </summary>
        private void SetDeleteBookState(bool isButtonVisible, bool isDeleteVisible)
        {
            IsDeleteBookButton = isButtonVisible;
            IsDeleteBook = isDeleteVisible;
        }

        /// <summary>
        /// Initiates the delete book process by showing the confirmation message
        /// </summary>
        private void DeleteBook(object obj)
        {
            SetDeleteBookState(false, true);
        }

        /// <summary>
        /// Cancels the delete book process and restores the button visibility
        /// </summary>
        private void CancelDeleteBook(object obj)
        {
            SetDeleteBookState(true, false);
        }

        /// <summary>
        /// Confirms and deletes the selected book asynchronously
        /// </summary>
        private async Task ConfirmDeleteBookAsync()
        {
            if (SelectedBook == null) return;
            await _bookProviders.DeleteAsync(SelectedBook);

            // Navigates back if possible
            if (_navigationViewModel.GoBackCommand.CanExecute(null))
            {
                _navigationViewModel.GoBackCommand.Execute(null);
            }

            SetDeleteBookState(true, false);
        }

        /// <summary>
        /// Cancels any unsaved changes and restores book details
        /// </summary>
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

        /// <summary>
        /// Saves changes to the selected book asynchronously
        /// </summary>
        private async Task CommitBookChangesAsync()
        {
            if (!string.IsNullOrWhiteSpace(Title) && SelectedBook != null)
            {
                var updatedBook = new Book { 
                    IdBook = SelectedBook.IdBook,
                    Title = Title,
                    Author = Author,
                    CountPages = _pageCount ?? 0,
                    PathImg = SelectedImagePath,
                    Description = BookDescription,
                    Rating= SelectedRating,
                    IdShelf = SelectedBook.IdShelf };
                await _bookProviders.UpdateAsync(updatedBook);
                SelectedBook = updatedBook;
            }
        }

        /// <summary>
        /// Checks if any book content has changed and updates the flag
        /// </summary>
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

        /// <summary>
        /// Opens a file dialog to select an FB2 or image file.
        /// </summary>
        private async void SelectFb2File(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "FB2 Files|*.fb2|Image Files|*.png;*.jpg",
                Title = "Select a File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                await ProcessSelectedFileAsync(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Processes the selected file (either FB2 or image).
        /// </summary>
        private async Task ProcessSelectedFileAsync(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            if (extension == ".fb2")
            {
                await ProcessFb2FileAsync(filePath);
            }
            else if (extension == ".png" || extension == ".jpg")
            {
                SaveImageFile(filePath);
            }
        }

        /// <summary>
        /// Parses the selected FB2 file and extracts book details.
        /// </summary>
        private async Task ProcessFb2FileAsync(string filePath)
        {
            var result = await Fb2Parser.ParseFb2FileAsync(filePath);

            Title = string.IsNullOrEmpty(result.bookTitle) ? "Unknown Title" : result.bookTitle;
            Author = string.IsNullOrEmpty(result.bookAuthor) ? "Unknown Author" : result.bookAuthor;

            BookDescription = string.IsNullOrEmpty(result.bookDescription)
                ? "No description available"
                : result.bookDescription.Length > 700
                    ? result.bookDescription.Substring(0, 700)
                    : result.bookDescription;

            SelectedImagePath = string.IsNullOrEmpty(result.imagePath) ? string.Empty : result.imagePath;
        }



        /// <summary>
        /// Saves the selected image file to a local directory.
        /// </summary>
        private void SaveImageFile(string filePath)
        {
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(filePath);
            string destinationPath = Path.Combine(imagesFolder, newFileName);
            File.Copy(filePath, destinationPath, true);
            SelectedImagePath = destinationPath;
        }
    }

}

