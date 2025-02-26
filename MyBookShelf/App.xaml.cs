using System.Windows;
using MyBookShelf.DBContext;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.BookGenreRroviders;
using MyBookShelf.Repositories.NoteProviders;
using MyBookShelf.Repositories.ReadingSessionProviders;
using MyBookShelf.Repositories.ShelfProviders;
using MyBookShelf.ViewModel;
using MyBookShelf.Services;
using MyBookShelf.DatabaseInitializer;
namespace MyBookShelf
{
    public partial class App : Application
    {
        private const string CONNECTION_ST = "connection";
        private readonly BookShelfDbContextFactory _bookShelfDbContextFactory;
        private readonly IBookProviders _bookProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        private readonly INoteProviders _noteProviders;
        private readonly IReadingSessionProviders _readingSessionProvider;
        private readonly IShelfProviders _shelfProvider;

        private readonly ICreator _creator;
        public App()
        {
            _bookShelfDbContextFactory = new BookShelfDbContextFactory(CONNECTION_ST);
            _bookProviders = new DatabaseBookProviders(_bookShelfDbContextFactory);
            _genreProviders = new DatabaseGenreProviders(_bookShelfDbContextFactory);
            _bookGenreProviders = new DatabaseBookGenreProviders(_bookShelfDbContextFactory);
            _noteProviders = new DatabaseNoteProviders(_bookShelfDbContextFactory);
            _readingSessionProvider = new DatabaseReadingSessionProviders(_bookShelfDbContextFactory);
            _shelfProvider = new DatabaseShelfProviders(_bookShelfDbContextFactory);
            _creator = new Creator(_shelfProvider,_bookProviders);
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
           using (BookShelfDBContext dbContext = _bookShelfDbContextFactory.CreateDbContext())
            {

                dbContext.Database.Migrate();
                await DbInitializer.InitializeGenres(_genreProviders);

            }

            var navigationViewModel = new NavigationViewModel(_creator,_shelfProvider,_bookProviders,_bookGenreProviders,_genreProviders);
            MainWindow = new MainWindow 
            {
               DataContext = navigationViewModel
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
