using PlantProtectionTechnologist.ApiGetCs;
using PlantProtectionTechnologist.Models;
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

namespace PlantProtectionTechnologist.Pages.ProductActions
{
    /// <summary>
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        private CircleAnimator _circleAnimator;
        private DispatcherTimer _timer;

        ConfirmationProduct newProduct;
        bool isLoaded = false;

        ApiClient apiClient = new ApiClient();

        VisualAddEditProduct visua;

        public Edit(List<ProductDto> dataProduction, ProductDto selectedProduct)
        {
            InitializeComponent();



           

            visua = new VisualAddEditProduct(dataProduction, typeProduct, releaseFormProduct)
            {
                textCode = selectedProduct.code,
                textName = selectedProduct.name.Split("\"")[1],
                confirmationTypeProduct = selectedProduct.type,
                confirmationReleaseForm = selectedProduct.releaseForm,
                confirmationComment = selectedProduct.comment

            };
            int index = Array.FindIndex(visua.prefixName, x =>string.Equals(x.prefixNameDecoding, visua.confirmationTypeProduct, StringComparison.OrdinalIgnoreCase));
            typeProduct.SelectedIndex = index;




            DataContext = visua;

            Loaded.Loaded += Window_Loaded;

            _circleAnimator = new CircleAnimator(Circle, orbitRadius: 40, centerX: 50, centerY: 50);
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(20);

            _timer.Tick += MoveCircle_Tick;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnpropertyChanget(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            confirmation.Visibility = Visibility.Collapsed;
            parentGrid.IsHitTestVisible = true;
            parentGrid.Effect = new BlurEffect() { Radius = 0 };
        }
        private async void Confirmation_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void Handling_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
