using Learning_Words.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MyBookShelf.Models;
namespace MyBookShelf.ViewModel
{
    public class BooksMainViewModel : ViewModelBase
    {
        public ObservableCollection<Shelf> Shelves { get; set; }
        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Book> FilteredBooks { get; set; }

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

                    if (Books != null)
                    {
                        LoadBooks();
                    }
                }
            }
        }


        public ICommand TemplateClickCommand { get; set; }
        public ICommand AddBookCommand { get; set; }

        public BooksMainViewModel()
        {
            Shelves = new ObservableCollection<Shelf>
                    {
                        new Shelf { IdShelf = -1, Name = "My book shelf" },
                        new Shelf { IdShelf = 2, Name = "Shelf one" },
                        new Shelf { IdShelf = 3, Name = "Shelf two" },
                        new Shelf { IdShelf = 4, Name = "Shelf three" },
                        new Shelf { IdShelf = 5, Name = "Shelf four" }
                    };

            Books = new ObservableCollection<Book>
                    {
                        new Book { IdBook = 1, Title = "Harry Potter", Author = "J.K. Rowling", PathImg = "Images/harry_potter.jpg", Rating = 5, IdShelf = 2 },
                        new Book { IdBook = 2, Title = "The Hobbit", Author = "J.R.R. Tolkien", PathImg = "Images/the_hobbit.jpg", Rating = 4, IdShelf = 3 },
                        new Book { IdBook = 3, Title = "1984", Author = "George Orwell", PathImg = "Images/1984.jpg", Rating = 5, IdShelf = 3 }, new Book { IdBook = 1, Title = "Harry Potter", Author = "J.K. Rowling", PathImg = "Images/harry_potter.jpg", Rating = 5, IdShelf = 2 },
                        new Book { IdBook = 2, Title = "The Hobbit", Author = "J.R.R. Tolkien", PathImg = "Images/the_hobbit.jpg", Rating = 4, IdShelf = 3 },
                        new Book { IdBook = 3, Title = "1984", Author = "George Orwell", PathImg = "Images/1984.jpg", Rating = 5, IdShelf = 3 }, new Book { IdBook = 1, Title = "Harry Potter", Author = "J.K. Rowling", PathImg = "Images/harry_potter.jpg", Rating = 5, IdShelf = 2 },
                        new Book { IdBook = 2, Title = "The Hobbit", Author = "J.R.R. Tolkien", PathImg = "Images/the_hobbit.jpg", Rating = 4, IdShelf = 3 },
                        new Book { IdBook = 3, Title = "1984", Author = "George Orwell", PathImg = "Images/1984.jpg", Rating = 5, IdShelf = 3 }, new Book { IdBook = 1, Title = "Harry Potter", Author = "J.K. Rowling", PathImg = "Images/harry_potter.jpg", Rating = 5, IdShelf = 2 },
                        new Book { IdBook = 2, Title = "The Hobbit", Author = "J.R.R. Tolkien", PathImg = "Images/the_hobbit.jpg", Rating = 4, IdShelf = 3 },
                        new Book { IdBook = 3, Title = "1984", Author = "George Orwell", PathImg = "Images/1984.jpg", Rating = 5, IdShelf = 3 }, new Book { IdBook = 1, Title = "Harry Potter", Author = "J.K. Rowling", PathImg = "Images/harry_potter.jpg", Rating = 5, IdShelf = 2 },
                        new Book { IdBook = 2, Title = "The Hobbit", Author = "J.R.R. Tolkien", PathImg = "Images/the_hobbit.jpg", Rating = 4, IdShelf = 3 },
                        new Book { IdBook = 3, Title = "1984", Author = "George Orwell", PathImg = "Images/1984.jpg", Rating = 5, IdShelf = 3 }
                    };

            FilteredBooks = new ObservableCollection<Book>(Books);

            SelectedShelf = Shelves[0];

            TemplateClickCommand = new RelayCommand(param =>
            {
                if (param is Book book)
                {
                    MessageBox.Show($"Clicked on {book.Title}");
                }
            });

            AddBookCommand = new RelayCommand(OpenAddBookWindow);
        }

        private void LoadBooks()
        {
            if (FilteredBooks == null)
                FilteredBooks = new ObservableCollection<Book>();

            FilteredBooks.Clear();

            if (SelectedShelf.IdShelf == -1)
            {
                foreach (var book in Books)
                {
                    FilteredBooks.Add(book);
                }
            }
            else
            {
                foreach (var book in Books.Where(b => b.IdShelf == SelectedShelf.IdShelf))
                {
                    FilteredBooks.Add(book);
                }
                FilteredBooks.Add(new Book { Title = "+", IdShelf = SelectedShelf.IdShelf });
            }
           
        }


        private void OpenAddBookWindow(object p)
        {
          
        }

        

    }

}