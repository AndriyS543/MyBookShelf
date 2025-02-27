using Learning_Words.Utilities;
using Microsoft.Win32;
using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.NoteProviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using MyBookShelf.Utilities;
using MyBookShelf.View;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MyBookShelf.ViewModel
{
    public class ReadingBookViewModel : ViewModelBase
    {
        // Dependencies for data management
        private readonly Shelf _shelf;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly INoteProviders _noteProviders;
        private readonly ICreator _creator;
        private readonly Book _selectedBook;
        private readonly DispatcherTimer _timer;
        private TimeSpan _elapsedTime;

        // Properties for book details
        private string? _tbCountPage;
        public string? tbCountPage
        {
            get => _tbCountPage;
            set
            {
                if (_tbCountPage != value)
                {
                    _tbCountPage = value;
                    OnPropertyChanged(); // Notify UI of changes
                }
            }
        }
        private int? _pageStart;
        public string? tbPageStartText
        {
            get => _pageStart?.ToString() ?? string.Empty;
            set
            {
                if (int.TryParse(value, out int result))
                    _pageStart = result;
                else
                    _pageStart = null;

                OnPropertyChanged();
 
            }
        }

        private string _notes ;
        public string Notes
        {
            get => _notes;
            set 
            {
                if (_notes != value) 
                {
                    _notes = value;
                    OnPropertyChanged();
                }            
            }
        }

        private int? _pageFinish;
        public string? tbPageFinishText
        {
            get => _pageFinish?.ToString() ?? string.Empty;
            set
            {
                if (int.TryParse(value, out int result))
                    _pageFinish = result;
                else
                    _pageFinish = null;

                OnPropertyChanged();

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

        private string? _imagePath;
        public string? imagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(); 
            }
        }
        public ObservableCollection<SelectableGenre> Genres { get; set; } = new ObservableCollection<SelectableGenre>();
        public ICommand CommitCommand { get; }

        // Constructor
        public ReadingBookViewModel(Book selectedBook,ICreator creator,IReadingSessionProviders readingSessionProviders,INoteProviders noteProviders)
        {
            _selectedBook = selectedBook;
            _creator = creator;
            _noteProviders = noteProviders;
            CommitCommand = new AsyncRelayCommand(CommitAsync);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            LoadData();
            StartTimer();
        }

        
        private async Task CommitAsync()
        {
            if (_pageStart >= _pageFinish )
            {
                // Якщо кінцева сторінка не більша за початкову, вийти з методу
                return;
            }
            if (_pageStart is null || _pageFinish is null)
            {
                // Якщо обидві сторінки не задані, вийти з методу
                return;
            }



            // Обчислення фінального відсотка
            int finishPercent = (int)((double)(_pageFinish-_pageStart) / _selectedBook.CountPages * 100);

            // Створення сесії читання
            var readingSession = await _creator.CreateReadingSessionAsync(_selectedBook.IdBook, _elapsedTime, _pageStart??0, _pageFinish??0, finishPercent);

            // Зупинка таймера
            StopTimer();

            // Створення нотатки
            if (!string.IsNullOrWhiteSpace(Notes)&& readingSession is not null)
            {
                Note newNote = new Note
                {
                    IdReadingSession = readingSession.IdReadingSession, // Заміниться на реальний ID після створення сесії
                    Text = Notes
                };

                // Тут потрібно зберегти нотатку (якщо є відповідний метод у `ICreator`)
                await _noteProviders.AddAsync(newNote);
            }
            var window = Application.Current.Windows.OfType<ReadingBook>().FirstOrDefault();
            window?.Close();
        }

        public string ElapsedTime => _elapsedTime.ToString(@"hh\:mm\:ss");


        private void Timer_Tick(object? sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(ElapsedTime));
        }

        private void LoadData()
        {
            tbBookTitle =_selectedBook.Title;
            _imagePath = _selectedBook.PathImg;
            _tbBookAuthor = _selectedBook.Author;
            _tbCountPage = _selectedBook.CountPages.ToString();
        }

        private void StartTimer()
        {
            _elapsedTime = TimeSpan.Zero; // Скидаємо лічильник
            _timer.Start();
            OnPropertyChanged(nameof(ElapsedTime));
        }
        private void StopTimer()
        {
            _timer.Stop();
        }
    }
}
