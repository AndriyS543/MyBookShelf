using Learning_Words.Utilities;
using MyBookShelf.Models;
using System.Collections.ObjectModel;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyBookShelf.ViewModel
{
    public class ShelvesViewModel : ViewModelBase
    {
        public ObservableCollection<Shelf> Shelves { get; set; }

        private Shelf _selectedShelf;
        public Shelf SelectedShelf
        {
            get => _selectedShelf;
            set
            {
                if (_selectedShelf != value)
                {
                    _selectedShelf = value;
                    OnPropertyChanged();

                  
                }
            }
        }
        public ShelvesViewModel() {
            Shelves = new ObservableCollection<Shelf>
                    {
                        new Shelf { IdShelf = -1, Name = "My book shelf" },
                        new Shelf { IdShelf = 2, Name = "Shelf one" },
                        new Shelf { IdShelf = 3, Name = "Shelf two" },
                        new Shelf { IdShelf = 4, Name = "Shelf three" },
                        new Shelf { IdShelf = 5, Name = "Shelf four" }
                    };
            SelectedShelf = Shelves[0];
        }
    }
}
