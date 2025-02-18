using System.Windows;
using MyBookShelf.DBContext;
using Microsoft.EntityFrameworkCore;
using MyBookShelf.Repositories.BookRroviders;
using MyBookShelf.Repositories.GenreRroviders;
using MyBookShelf.Repositories.BookGenreRroviders;
namespace MyBookShelf
{
    public partial class App : Application
    {
        private const string CONNECTION_ST = "connection";
        private readonly BookShelfDbContextFactory _bookShelfDbContextFactory;
        private readonly IBookProviders _bookProviders;
        private readonly IGenreProviders _genreProviders;
        private readonly IBookGenreProviders _bookGenreProviders;
        public App()
        {
            _bookShelfDbContextFactory = new BookShelfDbContextFactory(CONNECTION_ST);
            _bookProviders = new DatabaseBookProviders(_bookShelfDbContextFactory);
            _genreProviders = new DatabaseGenreProviders(_bookShelfDbContextFactory);
            _bookGenreProviders = new DatabaseBookGenreProviders(_bookShelfDbContextFactory);
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            using (BookShelfDBContext dbContext = _bookShelfDbContextFactory.CreateDbContext())
            {

                dbContext.Database.Migrate();

            }

            MainWindow = new MainWindow();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
