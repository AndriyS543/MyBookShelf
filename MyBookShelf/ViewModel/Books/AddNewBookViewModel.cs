using Microsoft.Win32;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Services;
using MyBookShelf.Utilities;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{
    public class AddNewBookViewModel : ViewModelBase
    {
        // Dependencies for data management
        private readonly Shelf _shelf;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly ICreator _creator;

        // Properties for book details
        private int? _tbCountPage;
        public string? tbCountPage
        {
            get => _tbCountPage.ToString() ?? string.Empty;
            set
            {
                if (int.TryParse(value, out int result))
                    _tbCountPage = result;
                else
                    _tbCountPage = null;

                OnPropertyChanged(); // Notify UI of changes
                
            }
        }

        private string? _tbBookAuthor;
        public string? tbBookAuthor
        {
            get => _tbBookAuthor;
            set
            {
                if (_tbBookAuthor != value)
                {
                    _tbBookAuthor = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _tbBookTitle;
        public string? tbBookTitle
        {
            get => _tbBookTitle;
            set
            {
                if (_tbBookTitle != value)
                {
                    _tbBookTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _selectedImagePath;
        public string? SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged();
            }
        }

        private string BookDescription = ""; // Stores book description

        public ObservableCollection<SelectableGenre> Genres { get; set; } = new ObservableCollection<SelectableGenre>();
        public ICommand SelectImageCommand { get; }
        public ICommand AddBooksCommand { get; }
        public ICommand ToggleGenreSelectionCommand { get; }

        // Constructor
        public AddNewBookViewModel(Shelf shelf, ICreator creator, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _shelf = shelf;
            _creator = creator;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;

            AddBooksCommand = new AsyncRelayCommand(AddBooksAsync);
            SelectImageCommand = new RelayCommand(SelectFb2File);
            ToggleGenreSelectionCommand = new RelayCommand(ToggleGenreSelection);

            _ = LoadGenresAsync(); // Load genres asynchronously
        }

        /// <summary>
        /// Loads available genres asynchronously.
        /// </summary>
        private async Task LoadGenresAsync()
        {
            var existingBookGenres = await _bookGenreProviders.GetAllAsync();
            var genres = await _genreProviders.GetAllAsync();

            Genres = new ObservableCollection<SelectableGenre>(
                genres.Select(genre => new SelectableGenre(genre, existingBookGenres))
            );
            OnPropertyChanged(nameof(Genres)); // Notify UI about changes
        }

        /// <summary>
        /// Toggles genre selection.
        /// </summary>
        private void ToggleGenreSelection(object p)
        {
            if (p is SelectableGenre genre)
            {
                genre.IsSelected = !genre.IsSelected;
                OnPropertyChanged(); // Notify UI of changes
            }
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        private async Task AddBooksAsync()
        {
            var newBook = await CreateBookAsync();
            await AssignGenresToBookAsync(newBook.IdBook);
        }

        /// <summary>
        /// Creates a new book entry asynchronously.
        /// </summary>
        private async Task<Book> CreateBookAsync()
        {
            return await _creator.CreateBookAsync(
                tbBookTitle ?? "New book",
                _tbCountPage ?? 0,
                _shelf.IdShelf,
                tbBookAuthor ?? "No author",
                BookDescription,
                SelectedImagePath ?? "",
                0);
        }

        /// <summary>
        /// Assigns selected genres to the created book.
        /// </summary>
        private async Task AssignGenresToBookAsync(int bookId)
        {
            if (bookId > 0)
            {
                var selectedGenres = Genres.Where(g => g.IsSelected).Select(g => g.Genre.IdGenre);
                foreach (var genreId in selectedGenres)
                {
                    await _bookGenreProviders.AddAsync(new BookGenre
                    {
                        IdBook = bookId,
                        IdGenre = genreId
                    });
                }
            }
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

            tbBookTitle = string.IsNullOrEmpty(result.bookTitle) ? "Unknown Title" : result.bookTitle;
            tbBookAuthor = string.IsNullOrEmpty(result.bookAuthor) ? "Unknown Author" : result.bookAuthor;

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
