using System;
using System.Linq;
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
        private Hotel _currentHotel = new Hotel();
        public AddEditPage(Hotel selectedHotel)
        {
            InitializeComponent();

            if (selectedHotel != null) _currentHotel = selectedHotel;

            DataContext                  = _currentHotel;
            ComboCountries.SelectedIndex = 0;
            ComboCountries.ItemsSource   = ToursAppEntities.GetContext().Countries.ToList();

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // StringBuilder checks errors:
            if (string.IsNullOrWhiteSpace(_currentHotel.Name))                       errors.AppendLine("Не корректное заполнение поля: Название отеля");
            if (string.IsNullOrWhiteSpace(_currentHotel.CountOfStars.ToString()))    errors.AppendLine("Не корректное заполнение поля: Количество звёзд");
            if (_currentHotel.CountOfStars < 1 || _currentHotel.CountOfStars > 5)    errors.AppendLine("Количество звёзд должно существовать в заданном интервале (1-5)");
            if (_currentHotel.Country == null)                                       errors.AppendLine("Не корректный выбор страны");

            if (errors.Length > 0) { MessageBox.Show(errors.ToString()); return; }

            if (_currentHotel.Id == 0) { 
                ToursAppEntities.GetContext().Hotels.Add(_currentHotel);
            }

            try
            {
                ToursAppEntities.GetContext().SaveChanges();
                MessageBox.Show("Сохранение успешно!");
            } 
            catch (Exception ex) { MessageBox.Show(ex.ToString());}
        }
    }
}
