using MyBookShelf.Models;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using MyBookShelf.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyBookShelf.ViewModel
{
    public class ShelvesViewModel : ViewModelBase
    {
        // Services for data access
        private readonly IShelfProviders _shelfProvider;
        private readonly ICreator _creator;

        // Collection for UI binding
        public ObservableCollection<Shelf> Shelves { get; set; } = new ObservableCollection<Shelf>();

        // Commands for UI interactions
        public ICommand AddNewShelfCommand { get; }
        public ICommand DeleteShelfCommand { get; }
        public ICommand CancelChangesShelfCommand { get; }
        public ICommand CommitChangesShelfCommand { get; }
        public ICommand YesDeleteShelfCommand { get; }
        public ICommand NoDeleteShelfCommand { get; }

        private Shelf _selectedShelf = new Shelf();
        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                if (_selectedShelf != value)
                {
                    _selectedShelf = value;
                    UpdateShelfDetails(); // Update UI details when shelf changes
                }
            }
        }

        private string _tbShelfName = "";
        public string tbShelfName
        {
            get => _tbShelfName;
            set
            {
                if (_tbShelfName != value)
                {
                    _tbShelfName = value;
                    OnPropertyChanged(nameof(tbShelfName));
                    CheckShelfContentChanged(); // Track content changes
                }
            }
        }

        private string _tbShelfDescription = "";
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

            _ = LoadShelvesAsync(); // Load shelves on initialization
        }

        /// <summary>
        /// Loads shelves from the data source and initializes default values.
        /// </summary>
        private async Task LoadShelvesAsync()
        {
            Shelves.Clear();
            var shelves = await _shelfProvider.GetAllAsync();
            var descriptionMyBookShelf = "A collection that contains all books across different shelves.";

            if (!shelves.Any()) // If no shelves exist, add a default one
            {
                Shelves.Add(new Shelf { Name = "My Book Shelf", IdShelf = -1, Books = new List<Book>(), Description = descriptionMyBookShelf });
            }
            else
            {
                var allBooks = shelves.Where(s => s.IdShelf != -1).SelectMany(s => s.Books).ToList();
                Shelves.Add(new Shelf { Name = "My Book Shelf", IdShelf = -1, Books = allBooks, Description = descriptionMyBookShelf });

                foreach (var shelf in shelves) Shelves.Add(shelf);
            }

            SelectedShelf = Shelves[0]; // Select the first shelf by default
        }

        // Reloads the shelves and selects a specific shelf
        private async Task ReloadShelvesAsync(Shelf newShelf)
        {
            await LoadShelvesAsync();
            SelectedShelf = Shelves.FirstOrDefault(s => s.IdShelf == newShelf?.IdShelf) ?? Shelves.First();
        }

        /// <summary>
        /// Adds a new shelf asynchronously.
        /// </summary>
        private async Task AddNewShelfAsync()
        {
            var newShelf = await _creator.CreateShelfAsync($"New shelf {Shelves.Count}", "");
            await ReloadShelvesAsync(newShelf);
        }

        /// <summary>
        /// Initiates shelf deletion process.
        /// </summary>
        private void DeleteShelf(object obj)
        {
            IsDeleteShelfButton = false;
            IsDeleteShelf = true;
        }

        /// <summary>
        /// Cancels shelf changes and restores original values.
        /// </summary>
        private void CancelShelfChanges(object obj)
        {
            if (SelectedShelf != null)
            {
                tbShelfName = SelectedShelf.Name;
                tbShelfDescription = SelectedShelf.Description;
            }
        }

        /// <summary>
        /// Commits changes made to the selected shelf.
        /// </summary>
        private async Task CommitShelfChangesAsync()
        {
            if (!string.IsNullOrWhiteSpace(tbShelfName) && SelectedShelf != null)
            {
                var updatedShelf = new Shelf { IdShelf = SelectedShelf.IdShelf, Name = tbShelfName, Description = tbShelfDescription };
                await _shelfProvider.UpdateAsync(updatedShelf);
                await ReloadShelvesAsync(updatedShelf);
            }
        }

        /// <summary>
        /// Confirms and deletes the selected shelf.
        /// </summary>
        private async Task ConfirmDeleteShelfAsync()
        {
            if (SelectedShelf?.IdShelf != -1 && SelectedShelf != null)
            {
                await _shelfProvider.DeleteAsync(SelectedShelf);
                _ = LoadShelvesAsync();
            }
            IsDeleteShelfButton = true;
            IsDeleteShelf = false;
        }

        /// <summary>
        /// Cancels the deletion process.
        /// </summary>
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

        /// <summary>
        /// Updates the details of the selected shelf.
        /// </summary>
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
