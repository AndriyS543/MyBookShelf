using System.Windows;
using System.Windows.Controls;


namespace MyBookShelf.View
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class ReadingBook : Window
    {
        public ReadingBook()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
