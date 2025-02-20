
using System.Windows.Input;
using Learning_Words.Utilities;

namespace MyBookShelf.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        private object? _currentView;
        public object? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand BooksCommand { get; set; }
        public ICommand ShelvesCommand { get; set; }

        public ICommand ReadingCommand { get; set; }
        private void BooksMain(object obj) => CurrentView = new BooksMainViewModel();
        private void Shelves(object obj) => CurrentView = new ShelvesViewModel();

        private void Reading(object obj) => CurrentView = new ReadingMainViewModel();

        public NavigationViewModel()
        {
            BooksCommand = new RelayCommand(BooksMain);
            ShelvesCommand = new RelayCommand(Shelves);
            ReadingCommand = new RelayCommand(Reading);

            // Startup Page
            CurrentView = new BooksMainViewModel();
        }
    }
}
