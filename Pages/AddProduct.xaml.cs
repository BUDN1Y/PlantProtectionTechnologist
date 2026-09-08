using PlantProtectionTechnologist.ApiGetCs;
using PlantProtectionTechnologist.Models;
using PlantProtectionTechnologist.Scipts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        private CircleAnimator _circleAnimator;
        private DispatcherTimer _timer;

        ConfirmationProduct newProduct;
        bool isLoaded = false;

        ApiClient apiClient = new ApiClient();

        VisualAddEditProduct visua;

        public AddProduct(List<ProductDto> dataProduction)
        {
            InitializeComponent();

            visua = new VisualAddEditProduct(dataProduction, typeProduct, releaseFormProduct);
            DataContext = visua;

            Loaded.Loaded += Window_Loaded;

            _circleAnimator = new CircleAnimator(Circle, orbitRadius: 40, centerX: 50, centerY: 50);
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(20);

            _timer.Tick += MoveCircle_Tick;       
        }

        public event PropertyChangedEventHandler? PropertyChanged;
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        private void CloseAddProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Navigate.tabFrame.Navigate(new Production());
        }

        private void typeProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            visua.typeProduct_SelectionChanged();         
        }
        private void releaseFormProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            if (!IsLoaded) return;
            visua.releaseFormProduct_SelectionChanged();
        }      
        private void MoveCircle_Tick(object sender, EventArgs e)
        {
            _circleAnimator.Move();
        }
        private void Handling_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn != null)
            {
                if (string.IsNullOrEmpty(nameProduct.Text))
                {
                    nameProduct.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4444"));
                    requiredField.Visibility = Visibility.Visible;
                    return;
                }
                if (visua.confirmationTypeProduct == "Отсутствует")
                {
                if (visua.confirmationTypeProduct == "Отсутствует")
                        visua.confirmationTypeProduct = null;
                }

                string name = $"{visua.confirmationTypeProduct} \"{nameProduct.Text}\"";
                visua.textName = name;
                visua.confirmationComment = commentsProduct.Text;
                string? tag = btn.Tag.ToString();

                newProduct = new ConfirmationProduct()
                {
                    code = visua.textCode,
                    name = name,
                    type = visua.confirmationTypeProduct,
                    releaseForm = visua.confirmationReleaseForm,
                    comment = visua.confirmationComment,
                    status = (tag == "Черновик") ? 1 : 9
                };
                visua.confirmationReleaseForm = (visua.confirmationReleaseForm == null) ? "Отсутствует" : visua.confirmationReleaseForm;
                visua.confirmationTypeProduct = (visua.confirmationTypeProduct == null) ? "Отсутствует" : visua.confirmationTypeProduct;

                visua.confirmationStatus = (tag == "Черновик") ? "Сохранить как черновик?" : "Отправить на согласование?";
                confirmation.Visibility = Visibility.Visible;
                parentGrid.IsHitTestVisible = false;
                parentGrid.Effect = new BlurEffect() { Radius = 15 };
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            confirmation.Visibility = Visibility.Collapsed;
            parentGrid.IsHitTestVisible = true;
            parentGrid.Effect = new BlurEffect() { Radius = 0 };
        }

        private async void Confirmation_Click(object sender, RoutedEventArgs e)
        {
            confirmation.Visibility = Visibility.Collapsed;
            _timer.Start();           
            Loaded.Visibility = Visibility.Visible;        
            parentGrid.IsHitTestVisible = false;
            await apiClient.AddNewProduct(newProduct);
            _timer.Stop();
            parentGrid.Effect = new BlurEffect() { Radius = 0 };
            Loaded.Visibility = Visibility.Collapsed;
            parentGrid.IsHitTestVisible = true;
            Navigate.tabFrame.Navigate(new Production());
        }
    }
}
