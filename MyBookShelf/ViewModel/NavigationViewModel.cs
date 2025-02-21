
using System.Windows.Input;
using Learning_Words.Utilities;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.Services;

namespace MyBookShelf.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly IShelfProviders _shelfProvider;
        private readonly ICreator _creator;

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
        private void Shelves(object obj) => CurrentView = new ShelvesViewModel(_creator,_shelfProvider);

        private void Reading(object obj) => CurrentView = new ReadingMainViewModel();

        public NavigationViewModel(ICreator creator, IShelfProviders shelfProviders)
        {
            BooksCommand = new RelayCommand(BooksMain);
            ShelvesCommand = new RelayCommand(Shelves);
            ReadingCommand = new RelayCommand(Reading);

            _shelfProvider = shelfProviders;
            _creator = creator;
            // Startup Page
            CurrentView = new BooksMainViewModel();
        }
    }
}
