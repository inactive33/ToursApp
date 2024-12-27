using System.Linq;
using System.Windows;
using ToursApp.Entities;
using ToursApp.Pages;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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

        private void ExportWord_Click(object sender, RoutedEventArgs e)
        {
            var allUsers = _context.Users.ToList(); // Загружаем пользователей
            var application = new Word.Application();
            Word.Document document = null;

            try
            {
                document = application.Documents.Add();

                AddUsersTable(document, allUsers);

                document.SaveAs2(@"D:\Users.docx");
                application.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта в Word: {ex.Message}");
            }
            finally
            {
                // Освобождение ресурсов
                if (document != null)
                {
                    Marshal.ReleaseComObject(document);
                }
                Marshal.ReleaseComObject(application);
            }
        }
        private void AddUsersTable(Word.Document document, List<User> allUsers)
        {
            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;

            // Создаем таблицу с заголовком и данными
            Word.Table usersTable = document.Tables.Add(tableRange, allUsers.Count + 1, 5);
            usersTable.Borders.InsideLineStyle = usersTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            usersTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            // Заголовки таблицы (добавляем только один раз)
            string[] headers = { "Логин", "Пароль", "Имя", "Фамилия", "Отчество" };
            for (int i = 0; i < headers.Length; i++)
            {
                Word.Range cellRange = usersTable.Cell(1, i + 1).Range;
                cellRange.Text = headers[i];
                cellRange.Bold = 1;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }

            // Данные пользователей (добавляем в строки после заголовков)
            for (int i = 0; i < allUsers.Count; i++)
            {
                var currentUser = allUsers[i];
                string[] userData = { currentUser.Login, currentUser.Password, currentUser.FirstName, currentUser.MiddleName, currentUser.LastName };

                for (int j = 0; j < userData.Length; j++)
                {
                    Word.Range cellRange = usersTable.Cell(i + 2, j + 1).Range; // Начинаем с 2-й строки
                    cellRange.Text = userData[j];
                }
            }

            usersTable.Columns.AutoFit();
        }
    }
}
