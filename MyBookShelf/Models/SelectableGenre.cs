using MyBookShelf.Utilities;


namespace MyBookShelf.Models
{
    /// <summary>
    /// Represents a selectable genre that can be associated with a book.
    /// </summary>
    public class SelectableGenre : ViewModelBase
    {
        private bool _isSelected;
        private readonly int _bookId;
        private readonly IEnumerable<BookGenre> _bookGenres = new List<BookGenre>();

        // Gets the genre associated with this instance.
        public Genre Genre { get; }

        // Gets or sets a value indicating whether the genre is selected.
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableGenre"/> class.
        /// </summary>
        public SelectableGenre(Genre genre,  IEnumerable<BookGenre>? bookGenres = null, int bookId = -1)
        {
            Genre = genre;
            if (bookId > -1) 
            {
                _bookId = bookId;
                _bookGenres = bookGenres ?? new List<BookGenre>();
                _isSelected = _bookGenres.Any(bg => bg.IdBook == _bookId && bg.IdGenre == genre.IdGenre);
            }     
        }
    }
}
