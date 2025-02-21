using Learning_Words.Utilities;
using MyBookShelf.Models;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{
    public class ShelvesViewModel : ViewModelBase
    {
        private readonly IShelfProviders _shelfProvider;
        private readonly ICreator _creator;

        public ObservableCollection<Shelf> Shelves { get; set; } = new ObservableCollection<Shelf>();

        public ICommand AddNewShelfCommand { get; }
        public ICommand DeleteShelfCommand { get; }
        public ICommand CancelChangesShelfCommand { get; }
        public ICommand CommitChangesShelfCommand { get; }
        public ICommand YesDeleteShelfCommand { get; }
        public ICommand NoDeleteShelfCommand { get; }

        private Shelf _selectedShelf;
        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                if (_selectedShelf != value)
                {
                    _selectedShelf = value;
                    UpdateShelfDetails();
                }
            }
        }

        private string _tbShelfName;
        public string tbShelfName
        {
            get => _tbShelfName;
            set
            {
                if (_tbShelfName != value)
                {
                    _tbShelfName = value;
                    OnPropertyChanged(nameof(tbShelfName));
                    CheckShelfContentChanged();
                }
            }
        }

        private string _tbShelfDescription;
        public string tbShelfDescription
        {
            get => _tbShelfDescription;
            set
            {
                if (_tbShelfDescription != value)
                {
                    _tbShelfDescription = value;
                    OnPropertyChanged(nameof(tbShelfDescription));
                    CheckShelfContentChanged();
                }
            }
        }

        private int _tbCountBooks;
        public int tbCountBooks
        {
            get => _tbCountBooks;
            set
            {
                if (_tbCountBooks != value)
                {
                    _tbCountBooks = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isShelfContentChanged = false;
        public bool IsShelfContentChanged
        {
            get => _isShelfContentChanged;
            set
            {
                if (_isShelfContentChanged != value)
                {
                    _isShelfContentChanged = value;
                    OnPropertyChanged(nameof(IsShelfContentChanged));
                }
            }
        }

        private bool _IsDeleteShelfButton = true;
        public bool IsDeleteShelfButton
        {
            get => _IsDeleteShelfButton;
            set
            {
                if (_IsDeleteShelfButton != value)
                {
                    _IsDeleteShelfButton = value;
                    OnPropertyChanged(nameof(IsDeleteShelfButton));
                }
            }
        }

        private bool _IsDeleteShelf = false;
        public bool IsDeleteShelf
        {
            get => _IsDeleteShelf;
            set
            {
                if (_IsDeleteShelf != value)
                {
                    _IsDeleteShelf = value;
                    OnPropertyChanged(nameof(IsDeleteShelf));
                }
            }
        }

        // Constructor to initialize the ViewModel and commands
        public ShelvesViewModel(ICreator creator, IShelfProviders shelfProviders)
        {
            _shelfProvider = shelfProviders;
            _creator = creator;

            AddNewShelfCommand = new AsyncRelayCommand(AddNewShelfAsync);
            DeleteShelfCommand = new RelayCommand(DeleteShelf);
            CancelChangesShelfCommand = new RelayCommand(CancelShelfChanges);
            CommitChangesShelfCommand = new AsyncRelayCommand(CommitShelfChangesAsync);
            YesDeleteShelfCommand = new AsyncRelayCommand(ConfirmDeleteShelfAsync);
            NoDeleteShelfCommand = new RelayCommand(CancelDeleteShelf);

            LoadShelvesAsync();
        }

        // Loads shelves from the data source and initializes default values
        private async Task LoadShelvesAsync()
        {
            Shelves.Clear();
            var shelves = await _shelfProvider.GetAllAsync();
            var descriptionMyBookShelf = "A collection that contains all books across different shelves.";
            if (!shelves.Any()) // If no shelves exist, add a default one
            {
                Shelves.Add(new Shelf { Name = "My Book Shelf", IdShelf = -1, Books = new List<Book>() ,Description= descriptionMyBookShelf });
            }
            else
            {
                var allBooks = shelves.Where(s => s.IdShelf != -1).SelectMany(s => s.Books).ToList();
                Shelves.Add(new Shelf { Name = "My Book Shelf", IdShelf = -1, Books = allBooks, Description = descriptionMyBookShelf });

                foreach (var shelf in shelves) Shelves.Add(shelf);
            }

            SelectedShelf = Shelves.FirstOrDefault(); // Select the first shelf by default
        }

        // Reloads the shelves and selects a specific shelf
        private async Task ReloadShelvesAsync(Shelf newShelf)
        {
            await LoadShelvesAsync();
            SelectedShelf = Shelves.FirstOrDefault(s => s.IdShelf == newShelf?.IdShelf) ?? Shelves.First();
        }

        // Adds a new shelf asynchronously
        private async Task AddNewShelfAsync()
        {
            var newShelf = await _creator.CreateShelfAsync($"New shelf {Shelves.Count}", "");
            await ReloadShelvesAsync(newShelf);
        }

        // Initiates shelf deletion process
        private void DeleteShelf(object obj)
        {
            IsDeleteShelfButton = false;
            IsDeleteShelf = true;
        }

        // Cancels shelf changes and restores original values
        private void CancelShelfChanges(object obj)
        {
            if (SelectedShelf != null)
            {
                tbShelfName = SelectedShelf.Name;
                tbShelfDescription = SelectedShelf.Description;
            }
        }

        // Commits changes made to the selected shelf
        private async Task CommitShelfChangesAsync()
        {
            if (!string.IsNullOrWhiteSpace(tbShelfName) && SelectedShelf != null)
            {
                var updatedShelf = new Shelf { IdShelf = SelectedShelf.IdShelf, Name = tbShelfName, Description = tbShelfDescription };
                await _shelfProvider.UpdateAsync(updatedShelf);
                await ReloadShelvesAsync(updatedShelf);
            }
        }

        // Confirms and deletes the selected shelf
        private async Task ConfirmDeleteShelfAsync()
        {
            if (SelectedShelf?.IdShelf != -1)
            {
                await _shelfProvider.DeleteAsync(SelectedShelf);
                LoadShelvesAsync();
            }
            IsDeleteShelfButton = true;
            IsDeleteShelf = false;
        }

        // Cancels the deletion process
        private void CancelDeleteShelf(object obj)
        {
            IsDeleteShelfButton = true;
            IsDeleteShelf = false;
        }

        // Checks if shelf content has been modified
        private void CheckShelfContentChanged()
        {
            if (SelectedShelf == null || SelectedShelf.IdShelf == -1) return;

            IsShelfContentChanged = tbShelfName != SelectedShelf.Name || tbShelfDescription != SelectedShelf.Description;
        }

        // Updates the details of the selected shelf
        private void UpdateShelfDetails()
        {
            tbShelfName = SelectedShelf?.Name ?? string.Empty;
            tbShelfDescription = SelectedShelf?.Description ?? string.Empty;
            tbCountBooks = SelectedShelf?.Books?.Count ?? 0;
            IsShelfContentChanged = false;
            OnPropertyChanged(nameof(SelectedShelf));
        }
    }
}
