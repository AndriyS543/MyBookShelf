using Learning_Words.Utilities;
using Microsoft.Win32;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using MyBookShelf.Utilities;
using MyBookShelf.View;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MyBookShelf.ViewModel
{


    public class ManageBookGenreViewModel : ViewModelBase
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

        private readonly int _idBook;
        private readonly IBookProviders _bookProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;

        public ICommand CommitCommand { get; }
        public ICommand ToggleGenreSelectionCommand { get; }


        public ManageBookGenreViewModel(int idBook, IBookGenreProviders bookGenreProviders, IGenreProviders genreProviders)
        {
            _idBook = idBook;
            _bookGenreProviders = bookGenreProviders;
            _genreProviders = genreProviders;

            CommitCommand = new AsyncRelayCommand(CommitAsync);
            ToggleGenreSelectionCommand = new RelayCommand(ToggleGenreSelection);

            _ = LoadGenresAsync();
        }

        private async Task LoadGenresAsync()
        {
            var existingBookGenres = await _bookGenreProviders.GetAllAsync();

            var genres = await _genreProviders.GetAllAsync();
            // Перетворюємо список жанрів у SelectableGenre
            Genres = new ObservableCollection<SelectableGenre>(
                genres.Select(genre => new SelectableGenre(genre, existingBookGenres,_idBook))
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
        private async Task CommitAsync()
        {
            if (_idBook > 0)
            {
                // Отримуємо всі жанри, які вже пов’язані з книгою
                var existingBookGenres = (await _bookGenreProviders.GetAllAsync())
                    .Where(bg => bg.IdBook == _idBook)
                    .ToList();

                // Отримуємо вибрані жанри (новий список)
                var selectedGenreIds = Genres
                    .Where(g => g.IsSelected)
                    .Select(g => g.Genre.IdGenre)
                    .ToList();

                // Визначаємо жанри, які потрібно додати (вибрані, але ще не існують у зв’язку)
                var genresToAdd = selectedGenreIds
                    .Except(existingBookGenres.Select(bg => bg.IdGenre))
                    .ToList();

                // Визначаємо жанри, які потрібно видалити (були в книзі, але тепер не вибрані)
                var genresToRemove = existingBookGenres
                    .Where(bg => !selectedGenreIds.Contains(bg.IdGenre))
                    .ToList();


                // Видаляємо жанри, які більше не вибрані
                foreach (var bookGenre in genresToRemove)
                {
                    await _bookGenreProviders.DeleteAsync(bookGenre);
                }

                // Додаємо нові вибрані жанри
                foreach (var genreId in genresToAdd)
                {
                    await _bookGenreProviders.AddAsync(new BookGenre
                    {
                        IdBook = _idBook,
                        IdGenre = genreId
                    });
                }
            }
            var window = Application.Current.Windows.OfType<ManageBookGenre>().FirstOrDefault();
            window?.Close();

        }


    }
}