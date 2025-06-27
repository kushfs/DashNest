using System.Windows;
using PersonalDashboard.Views;

namespace PersonalDashboard
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new NotesView(); // default screen
        }

        private void Notes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new NotesView();
        }

        private void Todo_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TodoView();
        }

        private void Events_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EventsView();
        }
    }
}
