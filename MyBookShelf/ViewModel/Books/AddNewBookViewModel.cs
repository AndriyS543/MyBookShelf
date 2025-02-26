using Learning_Words.Utilities;
using Microsoft.Win32;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using MyBookShelf.Utilities;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MyBookShelf.ViewModel
{


    public class AddNewBookViewModel : ViewModelBase
    {
        private string? _tbCountPage;
        public string? tbCountPage
        {
            get => _tbCountPage;
            set
            {
                if (_tbCountPage != value)
                {
                    _tbCountPage = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<SelectableGenre> Genres { get; set; }
        public ICommand SelectGenreCommand { get; }

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
        private string _tbBookAuthor;
        public string tbBookAuthor
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

        private string _tbBookTitle;
        public string tbBookTitle
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
        private readonly Shelf _shelf;
        private readonly IShelfProviders _shelfProvider;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly ICreator _creator;

        private string BookDescription = "";
        public ICommand SelectImageCommand { get; }
        public ICommand AddBooksCommand { get; }
        public ICommand ToggleGenreSelectionCommand { get; }


        public AddNewBookViewModel(Shelf shelf, ICreator creator, IBookProviders bookProviders, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _shelf = shelf;
            _creator = creator;
            _bookProviders = bookProviders;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;

            AddBooksCommand = new AsyncRelayCommand(AddBooksAsync);
            SelectImageCommand = new RelayCommand(SelectFb2File);
            ToggleGenreSelectionCommand = new RelayCommand(ToggleGenreSelection);

            _ = LoadGenresAsync();
        }

        private async Task LoadGenresAsync()
        {
            var existingBookGenres = await _bookGenreProviders.GetAllAsync();

            var genres = await _genreProviders.GetAllAsync();
            // Перетворюємо список жанрів у SelectableGenre
            Genres = new ObservableCollection<SelectableGenre>(
                genres.Select(genre => new SelectableGenre(genre, existingBookGenres))
            );
            OnPropertyChanged(nameof(Genres)); // Оновлюємо прив’язку у UI
        }

        private void ToggleGenreSelection(object p)
        {
            if (p is SelectableGenre genre)
            {
                genre.IsSelected = !genre.IsSelected;
                OnPropertyChanged();
            }
        }
        private async Task AddBooksAsync()
        {
            var newBook = await _creator.CreateBookAsync(tbBookTitle,
                Int32.Parse(_tbCountPage??"0"),
                _shelf.IdShelf,
                tbBookAuthor,
                BookDescription,
                 _selectedImagePath ?? null,
                 0);
            var bookId = newBook.IdBook;
            if (bookId > 0)
            {
                // Отримуємо вибрані жанри
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
        private void SelectImage(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select an Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName;
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(sourcePath);
                string destinationPath = Path.Combine(imagesFolder, newFileName);

                File.Copy(sourcePath, destinationPath, true);

                // Тут можна оновити властивість, якщо потрібно прив'язати зображення до UI
                SelectedImagePath = destinationPath;
            }
        }
        private async void SelectFb2File(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "FB2 Files|*.fb2|Image Files|*.png;*.jpg",
                Title = "Select a File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var result = await Fb2Parser.ParseFb2FileAsync(openFileDialog.FileName);
                string extension = Path.GetExtension(openFileDialog.FileName).ToLower();
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }
                if (extension == ".fb2")
                {
                    tbBookTitle = result.bookTitle;
                    tbBookAuthor = result.bookAuthor;
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