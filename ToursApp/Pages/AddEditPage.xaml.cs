using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ToursApp.Entities;

namespace ToursApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private User _currentUser = new User();
        public AddEditPage(User selectedUsers)
        {
            InitializeComponent();

            if (selectedUsers != null) _currentUser = selectedUsers;

            DataContext = _currentUser;

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // StringBuilder checks errors:
            //if (string.IsNullOrWhiteSpace(_currentUser.Name))                       errors.AppendLine("Не корректное заполнение поля: Название отеля");
            //if (string.IsNullOrWhiteSpace(_currentUser.CountOfStars.ToString()))    errors.AppendLine("Не корректное заполнение поля: Количество звёзд");
            //if (_currentUser.CountOfStars < 1 || _currentUser.CountOfStars > 5)    errors.AppendLine("Количество звёзд должно существовать в заданном интервале (1-5)");
            //if (_currentUser.Country == null)                                       errors.AppendLine("Не корректный выбор страны");

            if (errors.Length > 0) { MessageBox.Show(errors.ToString()); return; }

            if (_currentUser.ID == 0) {
                IS24_USER10Entities.GetContext().Users.Add(_currentUser);
            }

            try
            {
               // IS24_USER10Entities.GetContext().SaveChanges();
                MessageBox.Show("Сохранение успешно!");
            } 
            catch (Exception ex) { MessageBox.Show(ex.ToString());}
        }
    }
}
