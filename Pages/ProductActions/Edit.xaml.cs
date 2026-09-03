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

        ConfirmationProduct editProduct;
        bool isLoaded = false;

        ApiClient apiClient = new ApiClient();

        VisualAddEditProduct visua;

        private string? _tipeProductFirst;
        private string? _textCode;

        public Edit(List<ProductDto> dataProduction, ProductDto selectedProduct)
        {
            InitializeComponent();

            _tipeProductFirst = selectedProduct.type;
            _textCode = selectedProduct.code;
            visua = new VisualAddEditProduct(dataProduction, typeProduct, releaseFormProduct)
            {
                textCode = _textCode,
                textName = selectedProduct.name.Split("\"")[1],
                confirmationTypeProduct = selectedProduct.type,
                confirmationReleaseForm = selectedProduct.releaseForm,
                confirmationComment = selectedProduct.comment
            };

            int index = Array.FindIndex(visua.prefixName, x => string.Equals(x.prefixNameDecoding, visua.confirmationTypeProduct, StringComparison.OrdinalIgnoreCase)); //Ищет в combobox нужное слово
            typeProduct.SelectedIndex = index;
            index = Array.FindIndex(visua.releaseFormProductName, x => string.Equals(x, visua.confirmationReleaseForm, StringComparison.OrdinalIgnoreCase));
            releaseFormProduct.SelectedIndex = index;


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
            visua.typeProduct_SelectionChanged(_tipeProductFirst, _textCode);
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
            try
            {
                confirmation.Visibility = Visibility.Collapsed;
                _timer.Start();
                Loaded.Visibility = Visibility.Visible;
                parentGrid.IsHitTestVisible = false;

                await apiClient.EditProduct(editProduct);

                _timer.Stop();
                parentGrid.Effect = new BlurEffect() { Radius = 0 };
                Loaded.Visibility = Visibility.Collapsed;
                parentGrid.IsHitTestVisible = true;

                Navigate.tabFrame.Navigate(new Production());
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        private async void Handling_Click(object sender, RoutedEventArgs e)
        {
            Button btn = new Button();
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


                editProduct = new ConfirmationProduct()
                {
                    oldCode = _textCode,
                    code = visua.textCode,
                    name = name,
                    type = visua.confirmationTypeProduct,
                    releaseForm = visua.confirmationReleaseForm,
                    comment = visua.confirmationComment,
                };
                visua.confirmationReleaseForm = (visua.confirmationReleaseForm == null) ? "Отсутствует" : visua.confirmationReleaseForm;
                visua.confirmationTypeProduct = (visua.confirmationTypeProduct == null) ? "Отсутствует" : visua.confirmationTypeProduct;

                confirmation.Visibility = Visibility.Visible;
                parentGrid.IsHitTestVisible = false;
                parentGrid.Effect = new BlurEffect() { Radius = 15 };
            }
        }
    }
}
