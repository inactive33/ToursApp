using System.Windows;
using ToursApp.Pages;
namespace ToursApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Auth());
            Manager.MainFrame = MainFrame;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, System.EventArgs e)
        {
            if (MainFrame.CanGoBack) { 
                BtnBack.Visibility = Visibility.Visible;
            }
            else { 
                BtnBack.Visibility= Visibility.Hidden;
            }
        }
    }
}
