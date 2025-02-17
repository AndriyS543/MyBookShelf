using System.Windows;
using MyBookShelf.DBContext;
using Microsoft.EntityFrameworkCore;

namespace MyBookShelf
{
    public partial class App : Application
    {
        private const string CONNECTION_ST = "connection";
        private readonly BookShelfDbContextFactory _bookShelfDbContextFactory;

        public App()
        {
            _bookShelfDbContextFactory = new BookShelfDbContextFactory(CONNECTION_ST);
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
