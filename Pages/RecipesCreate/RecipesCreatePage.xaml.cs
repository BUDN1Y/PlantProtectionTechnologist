using DocumentFormat.OpenXml.Wordprocessing;
using PlantProtectionTechnologist.ApiGetCs;
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
using PlantProtectionTechnologist.Models;
using DocumentFormat.OpenXml.Packaging;
using System.ComponentModel;

namespace PlantProtectionTechnologist.Pages.RecipesCreate
{
    /// <summary>
    /// Логика взаимодействия для RecipesCreatePage.xaml
    /// </summary>
    public partial class RecipesCreatePage : Page, INotifyPropertyChanged
    {
        private CircleAnimator _circleAnimator;
        private DispatcherTimer _timer;

        ProductDto _selectedProduct;
        RecipesData[]? resultRecipes;
        RecipesData? selectedRecipes;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _name;
        public string? name { get => _name; set { _name = value; OnpropertyChanget(nameof(name)); } }

        private string? _code;
        public string? code { get => _code; set { _code = value; OnpropertyChanget(nameof(code)); } }

        private string? _activeRecipeFill;
        public string? activeRecipeFill { get => _activeRecipeFill; set { _activeRecipeFill = value; OnpropertyChanget(nameof(activeRecipeFill)); } }

        private string? _statusColor;
        public string? statusColor { get => _statusColor; set { _statusColor = value; OnpropertyChanget(nameof(statusColor)); } }

        private string? _statusName;
        public string? statusName { get => _statusName; set { _statusName = value; OnpropertyChanget(nameof(statusName)); } }



        private DateOnly? _dateCreate;
        public DateOnly? dateCreate { get => _dateCreate; set { _dateCreate = value; OnpropertyChanget(nameof(dateCreate)); } }

        private int? _version;
        public int? version { get => _version; set { _version = value; OnpropertyChanget(nameof(version)); } }

        private string? _authorName;
        public string? authorName { get => _authorName; set { _authorName = value; OnpropertyChanget(nameof(authorName)); } }
        public int idRecipe { get; set; }
        public int productId { get; set; }

        public string? comments { get; set; }
        public int statusId { get; set; }
        public int authorId { get; set; }
        public string statusNameRecipes { get; set; } = null!;
        public string statusColorRecipes { get; set; } = null!;

        public RecipesCreatePage(ProductDto selectedProduct)
        {
            InitializeComponent();
            DataContext = this;
            name = selectedProduct.name;
            code = selectedProduct.code;
            activeRecipeFill = selectedProduct.activeRecipeFill;
            statusColor = selectedProduct.statusColor;
            statusName = selectedProduct.statusName;
            _selectedProduct = selectedProduct;

            _circleAnimator = new CircleAnimator(Circle, orbitRadius: 40, centerX: 50, centerY: 50);
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(20);

            _timer.Tick += MoveCircle_Tick;

            GetDataDbRecipesDat();



        }

        protected void OnpropertyChanget(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void GetDataDbRecipesDat()
        {
            try
            {
                StartLoaded();
                ApiClient apiClient = new ApiClient();
                resultRecipes = await apiClient.GetDataRecipes();

                if (resultRecipes != null)
                {
                    selectedRecipes = resultRecipes.FirstOrDefault(x => x.id == _selectedProduct.id);
                    if (selectedRecipes != null)
                    {
                        if (selectedRecipes.creationDate != null)
                        {
                            dateCreate = selectedRecipes.creationDate;
                            StopLoaded();
                        }
                        else
                        {                          
                            errorCreateDate.Visibility = Visibility.Visible;
                            StopLoaded();
                        }
                        version = selectedRecipes.version;
                        authorName = selectedRecipes.authorName;
                        StopLoaded();
                    }
                    else
                    {
                        confirmationCard.Visibility = Visibility.Visible;
                        StopLoaded();
                        BlurEffectStart();
                        
                    }
                }
                else
                {
                    confirmationCard.Visibility = Visibility.Visible;
                    StopLoaded();
                    BlurEffectStart();
                    
                }             
            }
            catch
            {
                confirmationCard.Visibility = Visibility.Visible;
                StopLoaded();
                BlurEffectStart();
            }
        }

        //private async Task<RecipesData[]?> GetDataDbRawMaterial()
        //{
        //    try
        //    {
        //        StartLoaded();

        //        ApiClient apiClient = new ApiClient();

        //        //RawMaterial[] resultRawMaterial = await apiClient.GetDataRecipes();

        //        StopLoaded();

        //        return;
        //    }
        //    catch
        //    {
        //        StopLoaded();

        //        return;
        //    }
        //}

        //private async Task<RecipesData[]?> GetDataDbRawMaterial()
        //{
        //    try
        //    {
        //        StartLoaded();

        //        ApiClient apiClient = new ApiClient();

        //        //RawMaterial[] resultRawMaterial = await apiClient.GetDataRecipes();

        //        StopLoaded();

        //        return ;
        //    }
        //    catch
        //    {
        //        StopLoaded();

        //        return ;
        //    }
        //}

        private void StopLoaded()
        {
            _timer.Stop();
            BlurEffectEnd();
            Loaded.Visibility = Visibility.Collapsed;
        }

        private void StartLoaded()
        {
            _timer.Start();
            BlurEffectStart();
            Loaded.Visibility = Visibility.Visible;
        }

        private void BlurEffectStart()
        {
            blurBorder.Effect = new BlurEffect() { Radius = 5 };            
            blurBorder.IsHitTestVisible = false;
        }

        private void BlurEffectEnd()
        {
            blurBorder.IsHitTestVisible = true;
            blurBorder.Effect = new BlurEffect() { Radius = 0 };
        }

        private void MoveCircle_Tick(object sender, EventArgs e)
        {
            _circleAnimator.Move();
        }

        private void AddNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            confirmationCard.Visibility = Visibility.Collapsed;
            BlurEffectEnd();
            dateCreate = DateOnly.FromDateTime(DateTime.Today);
            authorName = ButtonManager.instance.fio;
        }
    }
}
