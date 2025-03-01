using MyBookShelf.Models;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.NoteProviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Services;
using MyBookShelf.Utilities;
using MyBookShelf.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MyBookShelf.ViewModel
{
    public class ReadingBookViewModel : ViewModelBase
    {
        // Dependencies for data management
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

        private string _notes = "";
        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged(); // Notify UI of changes
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
                    OnPropertyChanged(); // Notify UI of changes
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
                    OnPropertyChanged(); // Notify UI of changes
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
                OnPropertyChanged(); // Notify UI of changes
            }
        }

        // Collection of genres for selection
        public ObservableCollection<SelectableGenre> Genres { get; set; } = new ObservableCollection<SelectableGenre>();

        // Command to commit reading session
        public ICommand CommitCommand { get; }

        // Constructor
        public ReadingBookViewModel(Book selectedBook, ICreator creator, IReadingSessionProviders readingSessionProviders, INoteProviders noteProviders)
        {
            _selectedBook = selectedBook;
            _creator = creator;
            _noteProviders = noteProviders;
            CommitCommand = new AsyncRelayCommand(CommitAsync);

            // Initialize a timer to track reading duration
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Tick every second
            };
            _timer.Tick += Timer_Tick;

            LoadData(); // Load book details
            StartTimer(); // Start tracking reading time
        }

        // Asynchronous method to commit reading session
        private async Task CommitAsync()
        {
            if (_pageStart >= _pageFinish)
            {
                // If the finish page is not greater than the start page, exit
                return;
            }
            if (_pageStart is null || _pageFinish is null)
            {
                // If both pages are null, exit
                return;
            }

            // Calculate reading completion percentage
            int finishPercent = 0;
            if (_selectedBook.CountPages > 0)
            {
                finishPercent = (int)((double)(_pageFinish - _pageStart) / _selectedBook.CountPages * 100);
            }

            // Create a new reading session
            var readingSession = await _creator.CreateReadingSessionAsync(_selectedBook.IdBook, _elapsedTime, _pageStart ?? 0, _pageFinish ?? 0, finishPercent);

            // Stop the timer
            StopTimer();

            // Create a note if there is any text and the session was created successfully
            if (!string.IsNullOrWhiteSpace(Notes) && readingSession is not null)
            {
                Note newNote = new Note
                {
                    IdReadingSession = readingSession.IdReadingSession, // Will be replaced with real ID after session creation
                    Text = Notes
                };

                // Save the note using note provider
                await _noteProviders.AddAsync(newNote);
            }

            // Close the reading window
            var window = Application.Current.Windows.OfType<ReadingBook>().FirstOrDefault();
            window?.Close();
        }

        // Property to display elapsed time in hh:mm:ss format
        public string ElapsedTime => _elapsedTime.ToString(@"hh\:mm\:ss");

        // Timer tick event handler - increments elapsed time
        private void Timer_Tick(object? sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(ElapsedTime)); // Notify UI of changes
        }

        // Load book details into UI properties
        private void LoadData()
        {
            tbBookTitle = _selectedBook.Title;
            _imagePath = _selectedBook.PathImg;
            _tbBookAuthor = _selectedBook.Author;
            _tbCountPage = _selectedBook.CountPages.ToString();
        }

        // Start the timer and reset elapsed time
        private void StartTimer()
        {
            _elapsedTime = TimeSpan.Zero; // Reset timer
            _timer.Start();
            OnPropertyChanged(nameof(ElapsedTime));
        }

        // Stop the timer
        private void StopTimer()
        {
            _timer.Stop();
        }
    }
}
