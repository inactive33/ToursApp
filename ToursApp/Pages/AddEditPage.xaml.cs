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
        readonly private User _currentUser = new User();
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
            if (string.IsNullOrWhiteSpace(_currentUser.FirstName))                       errors.AppendLine("Не корректное заполнение поля: Имя");
            if (string.IsNullOrWhiteSpace(_currentUser.MiddleName))                      errors.AppendLine("Не корректное заполнение поля: Фамилия");
            if (string.IsNullOrWhiteSpace(_currentUser.LastName))                        errors.AppendLine("Не корректное заполнение поля: Отчество");
            if (string.IsNullOrWhiteSpace(_currentUser.Login))                           errors.AppendLine("Не корректное заполнение поля: Логин");
         // if (string.IsNullOrWhiteSpace(_currentUser.Password))                       errors.AppendLine("Не корректное заполнение поля: Пароль");


            if (errors.Length > 0) { MessageBox.Show(errors.ToString()); return; }
            
            if (_currentUser.ID == 0) {
                IS24_USER10Entities.GetContext().Users.Add(_currentUser);
            }

            try
            {
                IS24_USER10Entities.GetContext().SaveChanges();
                MessageBox.Show("Сохранение успешно!");
            } 
            catch (Exception ex) { MessageBox.Show(ex.ToString());}
        }
    }
}
