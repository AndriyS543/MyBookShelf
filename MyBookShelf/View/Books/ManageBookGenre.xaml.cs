using System.Windows;
using System.Windows.Controls;


namespace MyBookShelf.View
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class ManageBookGenre : Window
    {
        public ManageBookGenre()
        {
            InitializeComponent();

        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
