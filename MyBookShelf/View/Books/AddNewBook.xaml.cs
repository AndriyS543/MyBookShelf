﻿using System.Windows;
using System.Windows.Controls;


namespace MyBookShelf.View
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class AddNewBook : Window
    {
        public AddNewBook()
        {
            InitializeComponent();

        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
