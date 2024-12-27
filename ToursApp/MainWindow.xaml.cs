using System.Linq;
using System.Windows;
using ToursApp.Entities;
using ToursApp.Pages;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Runtime.InteropServices;

namespace ToursApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly private IS24_USER10Entities _context = new IS24_USER10Entities();
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AuthPage());
            Manager.MainFrame = MainFrame;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, System.EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                BtnBack.Visibility = Visibility.Visible;
            }
            else
            {
                BtnBack.Visibility = Visibility.Hidden;
            }
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var allUsers = _context.Users
                .ToList()
                .OrderBy(p => p.FullName())
                .ToList();

            var application = new Excel.Application
            {
                SheetsInNewWorkbook = allUsers.Count()
            };

            Excel.Workbook workbook = null;

            try
            {
                workbook = application.Workbooks.Add();

                for (int i = 0; i < allUsers.Count; i++)
                {
                    GenerateWorksheet(application, allUsers[i], i + 1);
                }

                application.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта в Excel: {ex.Message}");
            }
            finally
            {
                // Гарантируем освобождение ресурсов
                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }
                Marshal.ReleaseComObject(application);
            }
        }
        private void GenerateWorksheet(Excel.Application application, User user, int sheetIndex)
        {
            Excel.Worksheet worksheet = (Excel.Worksheet)application.Worksheets.Add();
            worksheet.Name = user.FullName();

            string[] headers = { "Login", "Password", "First Name", "Middle Name", "Last Name" };
            string[] userData = { user.Login, user.Password, user.FirstName, user.MiddleName, user.LastName };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[i + 1, 1] = headers[i];
                worksheet.Cells[i + 1, 2] = userData[i];
            }

            worksheet.Columns.AutoFit();
        }
    }
}
