using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToursApp.Entities;

namespace ToursApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для TasksPage.xaml
    /// </summary>
    public partial class TasksPage : Page
    {
        public TasksPage()
        {
            InitializeComponent();

            var allTypes = IS24_USER10Entities.GetContext().Executors.GroupBy(e => e.Grade).Select(g => g.FirstOrDefault()).ToList();

            if (!allTypes.Any(e => e.Grade == "Все типы")) {
                allTypes.Insert(0, new Executor
                {
                    Grade = "Все типы"
                });
            }


            ComboType.ItemsSource = allTypes.Distinct();

            CheckAtual.IsChecked = true;
            ComboType.SelectedIndex = 0;

            UpdateTasks();
        }

        public void UpdateTasks() {
            var currentTasks = IS24_USER10Entities.GetContext().Tasks.ToList();

            if (ComboType.SelectedIndex > 0 && ComboType.SelectedItem is Executor selectedExecutor) currentTasks = currentTasks.Where(p => p.ExecutorID == selectedExecutor.ID).ToList();

            currentTasks = currentTasks.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (!CheckAtual.IsChecked.Value) currentTasks = currentTasks.Where(p => p.IsDeleted).ToList();

            LViewTasks.ItemsSource = currentTasks;  
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTasks();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTasks();
        }

        private void CheckAtual_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTasks();
        }
    } 
}
