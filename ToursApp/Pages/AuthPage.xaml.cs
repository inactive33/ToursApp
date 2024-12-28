using System.Windows;
using System.Windows.Controls;

namespace ToursApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new DiagrammPage());
        }
    }
}
