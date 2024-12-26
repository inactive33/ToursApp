using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToursApp.Entities;

namespace ToursApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для HotelsPage
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            DGridUsers.ItemsSource = IS24_USER10Entities.GetContext().Users.ToList();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as User));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var hotelsForRemoving = DGridUsers.SelectedItems.Cast<User>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {hotelsForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            { 
                try 
                {
                    IS24_USER10Entities.GetContext().Users.RemoveRange(hotelsForRemoving);
                  //  IS24_USER10Entities.GetContext().SaveChanges();

                } catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                //IS24_USER10Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridUsers.ItemsSource = IS24_USER10Entities.GetContext().Users.ToList();
            }
        }

        private void ImportUsers()
        {
            var filePath = @"D:\Users.txt";
            var fileData = File.ReadAllLines(filePath);

            foreach (var lines in fileData)
            {
                var data = lines.Split('\t');

                var tempUser = new User
                {
                    Password = data[0].Replace("\"", "").ToString(),
                    FirstName = data[1].Replace("\"", "").ToString(),
                    MiddleName = data[2].Replace("\"", "").ToString(),
                    LastName = data[3].Replace("\"", "").ToString(),
                    Login = data[4].Replace("\"", "").ToString(),
                    IsDeleted = (data[5] == "0") ? false : true
                };

                IS24_USER10Entities.GetContext().Users.Add(tempUser);
                try
                {
                    IS24_USER10Entities.GetContext().SaveChanges();
                }
                catch { MessageBox.Show("Данные для пользователя уже загружены"); return; }
            }

        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            ImportUsers();
        }
    }
}
