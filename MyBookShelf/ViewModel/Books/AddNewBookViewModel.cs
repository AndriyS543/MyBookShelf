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
        public ObservableCollection<Genre> Genres { get; set; }
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

        public ICommand SelectImageCommand { get; }
        public ICommand AddBooksCommand { get; }

        public AddNewBookViewModel(Shelf shelf, ICreator creator, IBookProviders bookProviders, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _shelf = shelf;
            _creator = creator;
            _bookProviders = bookProviders;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;

            AddBooksCommand = new AsyncRelayCommand(AddBooksAsync);
            SelectImageCommand = new RelayCommand(SelectFb2File);


        }
        private async Task AddBooksAsync()
        {
            await _creator.CreateBookAsync(tbBookTitle,
                Int32.Parse(_tbCountPage??"0"),
                _shelf.IdShelf,
                tbBookAuthor,
                "Default Description",
                 _selectedImagePath ?? null,
                 0);

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
                Filter = "FB2 Files|*.fb2",
                Title = "Select an FB2 File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var result = await Fb2Parser.ParseFb2FileAsync(openFileDialog.FileName);
                tbBookTitle = result.bookTitle;
                tbBookAuthor = result.bookAuthor;
                SelectedImagePath = result.imagePath;
            }
        }

    }
}