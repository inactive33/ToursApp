using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToursApp.Entities;

namespace ToursApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для DiagrammPage.xaml
    /// </summary>
    public partial class DiagrammPage : Page
    {
        readonly private IS24_USER10Entities _context = new IS24_USER10Entities();
        public DiagrammPage()
        {
            InitializeComponent();

            ChartUsers.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Names")
            {
                IsValueShownAsLabel = true,
            };
            ChartUsers.Series.Add(currentSeries);

            ComboChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
            ComboUsers.ItemsSource = _context.Executors
                                    .Select(exec => exec.Grade)
                                    .Distinct()
                                    .ToList();
        }
        private void ComboUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboChartTypes.SelectedItem is SeriesChartType currentType &&
                ComboUsers.SelectedItem is string selectedGrade)
            {
                // Получаем серию
                Series currentSeries = ChartUsers.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                // Получение списка должностей
                var grades = _context.Executors
                    .Select(exec => exec.Grade)
                    .Distinct()
                    .ToList();

                // Группируем задачи по должностям исполнителей
                var taskExecutors = _context.Tasks
                    .Select(task => new
                    {
                        TaskTitle = task.Title,
                        ExecutorGrade = _context.Executors
                            .Where(exec => exec.ID == task.ExecutorID)
                            .Select(exec => exec.Grade)
                            .FirstOrDefault() // Предполагается, что одна задача связана с одним исполнителем
                    })
                    .ToList();

                // Устанавливаем текстовые подписи для оси Y
                ChartUsers.ChartAreas[0].AxisY.CustomLabels.Clear();
                for (int i = 0; i < grades.Count; i++)
                {
                    ChartUsers.ChartAreas[0].AxisY.CustomLabels.Add(
                        i - 0.5, i + 0.5, grades[i]
                    );
                }

                // Добавляем точки на диаграмму
                foreach (var item in taskExecutors)
                {
                    int gradeIndex = grades.IndexOf(item.ExecutorGrade);
                    if (gradeIndex >= 0) // Только если есть соответствие
                    {
                        currentSeries.Points.AddXY(item.TaskTitle, gradeIndex);
                    }
                }
            }
        }
    }
}