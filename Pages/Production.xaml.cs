using ClosedXML.Excel;
using CsvHelper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using PlantProtectionTechnologist.ApiGetCs;
using PlantProtectionTechnologist.Models;
using PlantProtectionTechnologist.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Data.DataTable;
using PlantProtectionTechnologist.Pages.ProductActions;
using PlantProtectionTechnologist.Scipts;

namespace PlantProtectionTechnologist
{
    /// <summary>
    /// Логика взаимодействия для Production.xaml
    /// </summary>
    public partial class Production : Page, INotifyPropertyChanged
    {
        private CircleAnimator _circleAnimator;
        private DispatcherTimer _timer;

        List<ProductDto> dataProduction = new List<ProductDto>();
        List<ProductDto> dataProductionBuffer = new List<ProductDto>();

        ApiClient apiClient = new ApiClient();

        string[] filterTypeName = new string[] { "Все типы", "Гербицид", "Инсектицид", "Фунгицид", "Регулятор роста", "Протравитель" };

        bool isLoaded = false;
        public Production()
        {
            InitializeComponent();
            DataContext = this;

            Loaded.Loaded += Window_Loaded;

            foreach (var item in filterTypeName)
            {
                filterType.Items.Add(item);
            }

            filterType.SelectedIndex = 0;

            _circleAnimator = new CircleAnimator(Circle, orbitRadius: 40, centerX: 50, centerY: 50);
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(20);

            _timer.Tick += MoveCircle_Tick;

            GetDataDb();
        }


        private string _test;
        public string test { get => _test; set { _test = value; OnpropertyChanget(nameof(test)); } }
        protected void OnpropertyChanget(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox el = (TextBox)sender;
            if (string.IsNullOrEmpty(el.Text))
            {
                textBoxSearchProduction.Visibility = Visibility.Visible;
            }
            else
            {
                textBoxSearchProduction.Visibility = Visibility.Hidden;
            }

            dataProduction = dataProductionBuffer.Where(x =>
            x.code.Contains(el.Text, StringComparison.OrdinalIgnoreCase) ||
            x.name.Contains(el.Text, StringComparison.OrdinalIgnoreCase)).ToList();

            productsDataGrid.ItemsSource = dataProduction;
        }

        private async void GetDataDb()
        {
            try
            {
                _timer.Start();
                blurBorder.Effect = new BlurEffect() { Radius = 5 };
                Loaded.Visibility = Visibility.Visible;
                blurBorder.IsHitTestVisible = false;

                ApiClient apiClient = new ApiClient();
                ProductDto[] result = await apiClient.GetDataProduction();
                dataProduction = result.ToList();
                dataProductionBuffer = dataProduction.ToList();

                productsDataGrid.ItemsSource = dataProduction;

                _timer.Stop();
                blurBorder.IsHitTestVisible = true;
                blurBorder.Effect = new BlurEffect() { Radius = 0 };
                Loaded.Visibility = Visibility.Collapsed;
            }
            catch
            {
                _timer.Stop();
                blurBorder.IsHitTestVisible = true;
                blurBorder.Effect = new BlurEffect() { Radius = 0 };
                Loaded.Visibility = Visibility.Collapsed;
            }
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!isLoaded) return;
            ComboBox el = (ComboBox)sender;


            string selectedType = el.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedType) || selectedType == "Все типы")
            {
                if (string.IsNullOrEmpty(searchName.Text))
                {
                    productsDataGrid.ItemsSource = dataProductionBuffer;
                }
                else
                {
                    productsDataGrid.ItemsSource = dataProduction;
                }
            }
            else
            {
                var filtered = dataProductionBuffer
            .Where(x => x.type != null &&
                       x.type.Equals(selectedType, StringComparison.OrdinalIgnoreCase))
            .ToList();

                productsDataGrid.ItemsSource = filtered;
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Navigate.tabFrame.Navigate(new AddProduct(dataProductionBuffer));
        }

        private void MoveCircle_Tick(object sender, EventArgs e)
        {
            _circleAnimator.Move();
        }

        private void ManagerExport_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            string tag = btn.Tag.ToString();
            ManagerExpor(tag);
        }

        private void ManagerExpor(string? tag)
        {
            if (tag == "0")
            {
                selecteExport.Visibility = Visibility.Visible;
                blurBorder.Effect = new BlurEffect() { Radius = 5 };
                blurBorder.IsHitTestVisible = false;
            }
            else
            {
                selecteExport.Visibility = Visibility.Collapsed;
                blurBorder.Effect = new BlurEffect() { Radius = 0 };
                blurBorder.IsHitTestVisible = true;
            }
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog() { Filter = "Excel|*.xlsx" };
            if (dialog.ShowDialog() != true) return;

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Data");

            var data = productsDataGrid.ItemsSource as IEnumerable<object>;

            if (data == null || !data.Any())
            {
                MessageBox.Show("Нет данных для экспорта!");
                return;
            }

            sheet.Cell(1, 1).InsertTable(data);
            sheet.Columns().AdjustToContents();
            workbook.SaveAs(dialog.FileName);

            successfully.Visibility = Visibility.Visible;
            selecteExport.Visibility = Visibility.Collapsed;


        }

        private void CloseExport_Click(object sender, RoutedEventArgs e)
        {
            successfully.Visibility = Visibility.Collapsed;
            ManagerExpor("1");
        }

        private void CSV_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "CSV files (*.csv)|*.csv",
            };


            if (dialog.ShowDialog() != true) return;

            try
            {
                var data = productsDataGrid.ItemsSource as IEnumerable<object>;

                if (data != null)
                {
                    if (!data.Any())
                    {
                        MessageBox.Show("Нет данных!");
                        return;
                    }
                }
                using var write = new StreamWriter(dialog.FileName);
                using var csv = new CsvWriter(write, CultureInfo.InvariantCulture);

                csv.WriteRecords(data);

                successfully.Visibility = Visibility.Visible;
                selecteExport.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private async void ManagerActions(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var selectedProduct = btn.DataContext as ProductDto;

            if (btn != null)
            {
                int? tag = Convert.ToInt32(btn.Tag);

                if (tag == 2 || tag == 3 || tag == 9)
                {
                    ConfirmationProduct product = new ConfirmationProduct()
                    {
                        recipe = selectedProduct.activeRecipeId,
                        techcard = selectedProduct.activeTechMapId,
                        id = selectedProduct.id,
                        code = selectedProduct.code,
                        oldStatus = selectedProduct.statusId,
                        status = tag.Value,
                        changetBy = (ButtonManager.instance.user == null) ? 1 : ButtonManager.instance.user.id
                    };
                    await apiClient.ChangetStatusProduct(product);
                    Navigate.tabFrame.Navigate(new Production());
                    return;
                }

                switch (tag)
                {
                    case 0:
                        Navigate.tabFrame.Navigate(new Viewing(selectedProduct));
                        break;

                    case 1:
                        Navigate.tabFrame.Navigate(new Edit(dataProduction, selectedProduct));
                        break;



                    default:
                        MessageBox.Show("Отсутствует");
                        break;
                }


                //9 удалить 2 восстановить 3 подтвердить
            }
        }
    }
}
