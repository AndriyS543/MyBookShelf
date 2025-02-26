using Learning_Words.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace MyBookShelf.Models
{
    public class SelectableGenre : ViewModelBase
    {
        private bool _isSelected;
        private readonly int _bookId;
        private readonly IEnumerable<BookGenre> _bookGenres;
        public Genre Genre { get; }

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

        public SelectableGenre(Genre genre,  IEnumerable<BookGenre> bookGenres = null,int bookId = -1)
        {
            Genre = genre;
            if (bookId > -1) 
            {
                _bookId = bookId;
                _bookGenres = bookGenres;
                _isSelected = _bookGenres.Any(bg => bg.IdBook == _bookId && bg.IdGenre == genre.IdGenre);
            }
            
        }

    }

}
