using MyBookShelf.Utilities;
using MyBookShelf.Models;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyBookShelf.ViewModel
{
    public class InfoViewModel : ViewModelBase
    {
        // GIF paths for different tutorials
        private string _gifCreateShelf;
        public string GifCreateShelf
        {
            get => _gifCreateShelf;
            set { _gifCreateShelf = value; OnPropertyChanged(); }
        }

        private string _gifAddBook;
        public string GifAddBook
        {
            get => _gifAddBook;
            set { _gifAddBook = value; OnPropertyChanged(); }
        }

        private string _gifReadingSession;
        public string GifReadingSession
        {
            get => _gifReadingSession;
            set { _gifReadingSession = value; OnPropertyChanged(); }
        }

        private string _gifEditBook;
        public string GifEditBook
        {
            get => _gifEditBook;
            set { _gifEditBook = value; OnPropertyChanged(); }
        }

        private string _gifNotes;
        public string GifNotes
        {
            get => _gifNotes;
            set { _gifNotes = value; OnPropertyChanged(); }
        }

        // Indicates if the GIFs are loading
        private bool _isGifLoading;
        public bool IsGifLoading
        {
            get => _isGifLoading;
            set { _isGifLoading = value; OnPropertyChanged(); }
        }

        // Constructor initializes GIF loading
        public InfoViewModel()
        {
            LoadGifAsync();
        }

        // Asynchronously loads GIF paths
        private void LoadGifAsync()
        {
            IsGifLoading = true; 
            Task.Run(() =>
            {
                // Define the file paths for the tutorial GIFs
                string gifPath_shelf = "pack://application:,,,/Images/Info/tutorial_create_shelf.gif";
                string gifPath_add_book = "pack://application:,,,/Images/Info/tutorial_add_book.gif";
                string gifPath_reading_session = "pack://application:,,,/Images/Info/tutorial_reading_session.gif";
                string gifPath_edit_book = "pack://application:,,,/Images/Info/tutorial_edit_book.gif";
                string gifPath_notes = "pack://application:,,,/Images/Info/tutorial_notes.gif";

                // Convert paths to URIs and assign them to properties
                GifCreateShelf = new Uri(gifPath_shelf, UriKind.RelativeOrAbsolute).ToString();
                GifAddBook = new Uri(gifPath_add_book, UriKind.RelativeOrAbsolute).ToString();
                GifReadingSession = new Uri(gifPath_reading_session, UriKind.RelativeOrAbsolute).ToString();
                GifEditBook = new Uri(gifPath_edit_book, UriKind.RelativeOrAbsolute).ToString();
                GifNotes = new Uri(gifPath_notes, UriKind.RelativeOrAbsolute).ToString();
                
                IsGifLoading = false;
            });
        }
    }
}
