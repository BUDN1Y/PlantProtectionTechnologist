using Microsoft.IdentityModel.Tokens;
using PlantProtectionTechnologist.ApiGetCs;
using PlantProtectionTechnologist.Pages.RecipesCreate;
using PlantProtectionTechnologist.Scipts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PlantProtectionTechnologist.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recipes.xaml
    /// </summary>
    public partial class Recipes : Page
    {
        VisualAddEditRecipes visualAddEditRecipes;
        VisualAddEditProduct visualAddEditProduct;
        List<ProductDto> dataProduct = new List<ProductDto>();
        List<ProductDto> dataProductBuffer = new List<ProductDto>();
        bool isLoaded = false;
        int selectedIndexTypeProduct = 0;
        int selectedIndexStatusProduct = 0;
        string selectedWordTypeProduct = "";
        string selectedWordStatusProduct = "";

        private CircleAnimator _circleAnimator;
        private DispatcherTimer _timer;
        public Recipes()
        {
            InitializeComponent();
            Loaded.Loaded += Window_Loaded;
            DataContext = this;
            visualAddEditProduct = new VisualAddEditProduct(typeProduct, statusProduct);

            _circleAnimator = new CircleAnimator(Circle, orbitRadius: 40, centerX: 50, centerY: 50);
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(20);

            _timer.Tick += MoveCircle_Tick;
            GetDataDb();
            Navigate.pageRecipesFrame = recipesFrame;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }
        private void MoveCircle_Tick(object sender, EventArgs e)
        {
            _circleAnimator.Move();
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
                ProductDto[]? result = await apiClient.GetDataProduction();
                if (result != null)
                {
                    dataProduct = result.ToList();
                    dataProductBuffer = dataProduct.ToList();

                    recipesDataGrid.ItemsSource = dataProduct;
                }
                else
                {
                    errorLoadedDB.Visibility = Visibility.Visible;
                }
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

            ApplyFilters();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var selectedProduct = recipesDataGrid.SelectedItem as ProductDto;
            Navigate.pageRecipesFrame.Navigate(new RecipesCreatePage(selectedProduct));
        }

        private void typeProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded) return;
            ComboBox el = (ComboBox)sender;
            selectedIndexTypeProduct = el.SelectedIndex;
            selectedWordTypeProduct = el.SelectedItem.ToString();
           
            ApplyFilters();
        }

        private void statusRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded) return;
            ComboBox el = (ComboBox)sender;

            selectedIndexStatusProduct = el.SelectedIndex;
            selectedWordStatusProduct = el.SelectedItem.ToString();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            bool isSeacrh = false;
            bool isTypeProduct = false;
            bool isStatusProduct = false;

            dataProductBuffer = dataProduct.ToList();
            if (!string.IsNullOrEmpty(searchName.Text))
            {
                dataProductBuffer = dataProductBuffer.Where(x =>
                    x.code.Contains(searchName.Text, StringComparison.OrdinalIgnoreCase) ||
                    x.name.Contains(searchName.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                isSeacrh = true;
            }

            if (selectedIndexTypeProduct != 0)
            {
                dataProductBuffer = dataProductBuffer.Where(x =>
                    x.type.Contains(selectedWordTypeProduct, StringComparison.OrdinalIgnoreCase)).ToList();
                isTypeProduct = true;
            }

            if (selectedIndexStatusProduct != 0)
            {
                dataProductBuffer = dataProductBuffer.Where(x =>
                    x.statusName.Contains(selectedWordStatusProduct, StringComparison.OrdinalIgnoreCase)).ToList();
                isStatusProduct = true;
            }

            if(!(isSeacrh && isTypeProduct && isStatusProduct))
            {
                recipesDataGrid.ItemsSource = dataProduct.ToList();
            }
                    

            recipesDataGrid.ItemsSource = dataProductBuffer.ToList();
        }
    }
}
