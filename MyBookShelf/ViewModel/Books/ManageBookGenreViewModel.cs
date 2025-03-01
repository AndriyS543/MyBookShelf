using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Utilities;
using MyBookShelf.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{
    public class ManageBookGenreViewModel : ViewModelBase
    {
        // Property to store the number of pages as a string
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

        // Collection of genres for UI binding
        public ObservableCollection<SelectableGenre> Genres { get; set; } = new();
        // Private fields for managing book and genre data
        private readonly int _idBook;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;

        // Commands for committing and toggling genre selection
        public ICommand CommitCommand { get; }
        public ICommand ToggleGenreSelectionCommand { get; }

        /// <summary>
        /// Constructor initializes dependencies and commands
        /// </summary>
        public ManageBookGenreViewModel(int idBook, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _idBook = idBook;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;

            CommitCommand = new AsyncRelayCommand(CommitAsync);
            ToggleGenreSelectionCommand = new RelayCommand(ToggleGenreSelection);

            _ = LoadGenresAsync(); // Load genres asynchronously
        }

        /// <summary>
        /// Loads available genres and maps them to a selectable format
        /// </summary>
        private async Task LoadGenresAsync()
        {
            var existingBookGenres = await _bookGenreProviders.GetAllAsync();
            var genres = await _genreProviders.GetAllAsync();

            // Convert genre list to SelectableGenre objects
            Genres = new ObservableCollection<SelectableGenre>(
                genres.Select(genre => new SelectableGenre(genre, existingBookGenres, _idBook))
            );
            OnPropertyChanged(nameof(Genres)); // Notify UI about the change
        }

        /// <summary>
        /// Toggles the selection state of a genre
        /// </summary>
        private void ToggleGenreSelection(object p)
        {
            if (p is SelectableGenre genre)
            {
                genre.IsSelected = !genre.IsSelected;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Commits the selected genres to the database
        /// </summary>
        private async Task CommitAsync()
        {
            if (_idBook > 0)
            {
                // Retrieve existing genres linked to the book
                var existingBookGenres = (await _bookGenreProviders.GetAllAsync())
                    .Where(bg => bg.IdBook == _idBook)
                    .ToList();

                // Get newly selected genre IDs
                var selectedGenreIds = Genres
                    .Where(g => g.IsSelected)
                    .Select(g => g.Genre.IdGenre)
                    .ToList();

                // Determine which genres need to be added
                var genresToAdd = selectedGenreIds
                    .Except(existingBookGenres.Select(bg => bg.IdGenre))
                    .ToList();

                // Determine which genres need to be removed
                var genresToRemove = existingBookGenres
                    .Where(bg => !selectedGenreIds.Contains(bg.IdGenre))
                    .ToList();

                // Remove unselected genres
                foreach (var bookGenre in genresToRemove)
                {
                    await _bookGenreProviders.DeleteAsync(bookGenre);
                }

                // Add newly selected genres
                foreach (var genreId in genresToAdd)
                {
                    await _bookGenreProviders.AddAsync(new BookGenre
                    {
                        IdBook = _idBook,
                        IdGenre = genreId
                    });
                }
            }

            // Close the window after committing changes
            var window = Application.Current.Windows.OfType<ManageBookGenre>().FirstOrDefault();
            window?.Close();

        }
    }
}