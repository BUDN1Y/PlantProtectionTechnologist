using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PlantProtectionTechnologist.Converts;
using PlantProtectionTechnologist.Models.Product;
using PlantProtectionTechnologist.Models.Recipe;
using PlantProtectionTechnologist.ModelsDB;
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

namespace PlantProtectionTechnologist.Pages.RecipesCreate
{
    /// <summary>
    /// Логика взаимодействия для RecipesCreatePage.xaml
    /// </summary>
    public partial class RecipesCreatePage : Page, INotifyPropertyChanged
    {
        private CircleAnimator _circleAnimator;
        private DispatcherTimer _timer;
        ApiClient apiClient = new ApiClient();

        ProductDto _selectedProduct;
        RecipesData[]? resultRecipes;
        RecipesData? selectedRecipe;
        RecipeComponets[]? recipeComponets;
        RawMaterialsData[]? rawMaterialsData { get; set; }
        List<RecipesDataGrid> recipesDataGrid;
        ComboBox recipeCmb;


        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _name;
        public string? name { get => _name; set { _name = value; OnpropertyChanget(nameof(name)); } }

        private string? _code;
        public string? code { get => _code; set { _code = value; OnpropertyChanget(nameof(code)); } }

        private string? _activeRecipeFill;
        public string? activeRecipeFill { get => _activeRecipeFill; set { _activeRecipeFill = value; OnpropertyChanget(nameof(activeRecipeFill)); } }

        private string? _statusColorProduct;
        public string? statusColorProduct { get => _statusColorProduct; set { _statusColorProduct = value; OnpropertyChanget(nameof(statusColorProduct)); } }

        private string? _statusNameProduct;
        public string? statusNameProduct { get => _statusNameProduct; set { _statusNameProduct = value; OnpropertyChanget(nameof(statusNameProduct)); } }



        private DateOnly? _dateCreate;
        public DateOnly? dateCreate { get => _dateCreate; set { _dateCreate = value; OnpropertyChanget(nameof(dateCreate)); } }

        private int? _version;
        public int? version { get => _version; set { _version = value; OnpropertyChanget(nameof(version)); } }

        private string? _authorName;
        public string? authorName { get => _authorName; set { _authorName = value; OnpropertyChanget(nameof(authorName)); } }

        private string? _statusColorRecipe;
        public string? statusColorRecipe { get => _statusColorRecipe; set { _statusColorRecipe = value; OnpropertyChanget(nameof(statusColorRecipe)); } }

        private string? _statusNameRecipe;
        public string? statusNameRecipe { get => _statusNameRecipe; set { _statusNameRecipe = value; OnpropertyChanget(nameof(statusNameRecipe)); } }

        private string? _comment;
        public string? comment { get => _comment; set { _comment = value; OnpropertyChanget(nameof(comment)); } }

        

        private string _createCode;
        public string createCode { get => _createCode; set { _createCode = value; OnpropertyChanget(nameof(createCode)); } }

        private decimal _valueProgressBar;
        public decimal valueProgressBar { get => _valueProgressBar; set { _valueProgressBar = value; OnpropertyChanget(nameof(valueProgressBar)); } }

        private string _valueTextBlock;
        public string valueTextBlock { get => _valueTextBlock; set { _valueTextBlock = value; OnpropertyChanget(nameof(valueTextBlock)); } }

        private string _selectedNameCommponent;
        public string selectedNameCommponent { get => _selectedNameCommponent; set { _selectedNameCommponent = value; OnpropertyChanget(nameof(selectedNameCommponent)); } }

        public RecipesCreatePage(ProductDto selectedProduct)
        {
            InitializeComponent();
            DataContext = this;
            name = selectedProduct.name;
            code = selectedProduct.code;
            activeRecipeFill = selectedProduct.activeRecipeFill;
            statusColorProduct = selectedProduct.statusColor;
            statusNameProduct = selectedProduct.statusName;
            _selectedProduct = selectedProduct;

            _circleAnimator = new CircleAnimator(Circle, orbitRadius: 40, centerX: 50, centerY: 50);
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(20);

            _timer.Tick += MoveCircle_Tick;

            GetDataDbRecipes();


        }

        protected void OnpropertyChanget(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void GetDataDbRecipes()
        {
            try
            {
                StartLoaded();
                resultRecipes = await apiClient.GetDataRecipes();

                if (resultRecipes != null)
                {
                    selectedRecipe = resultRecipes.FirstOrDefault(x => x.id == _selectedProduct.id);
                    if (selectedRecipe != null && _selectedProduct.activeRecipeId != null)
                    {
                        FillRecipseDataGrid();
                        FillRecipesComment();
                        if (selectedRecipe.creationDate != null)
                        {
                            dateCreate = selectedRecipe.creationDate;
                            StopLoaded();
                        }
                        else
                        {
                            errorCreateDate.Visibility = Visibility.Visible;
                            StopLoaded();
                        }
                        version = selectedRecipe.version;
                        authorName = selectedRecipe.authorName;
                        comment = selectedRecipe.comments;
                        statusColorRecipe = selectedRecipe.statusColor;
                        statusNameRecipe = selectedRecipe.statusName;

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

        private async Task<RawMaterialsData[]?> GetDataDbRawMaterials()
        {
            try
            {
                return await apiClient.GetDataDbRawMaterials();
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки материалов");
                return null;
            }
        }

        private async Task<RecipeComponets[]?> GetDataDBRecipeComponents(int id)
        {
            try
            {
                return await apiClient.GetDataRecipeComponets(id);
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки коммпонентов рецепта");
                return null;
            }
        }

        private async void FillRecipseDataGrid()
        {
            rawMaterialsData = await GetDataDbRawMaterials();
            recipeComponets = await GetDataDBRecipeComponents(selectedRecipe.id);

            foreach (var item in rawMaterialsData)
            {
                rawMterialsName.Items.Add(item.name);
            }


            recipesDataGrid = new List<RecipesDataGrid>();

            if (rawMaterialsData != null && recipeComponets != null)
            {
                for (int i = 0; i < recipeComponets.Length; i++)
                {
                    int rawMaterial = rawMaterialsData.FirstOrDefault(x => x.id == recipeComponets[i].rawMaterialId).id;
                    decimal tolerance = (recipeComponets[i].toleranceMax - recipeComponets[i].toleranceMin) / 2;
                    tolerance = Math.Round(tolerance, 2);
                    var recipe = new RecipesDataGrid()
                    {
                        id = i + 1,
                        code = rawMaterialsData[rawMaterial].code,
                        name = rawMaterialsData[rawMaterial].name,
                        percentage = recipeComponets[i].percentage,
                        loadOrder = recipeComponets[i].loadOrder,
                        tolerance = $"±{Convert.ToString(tolerance)}"
                    };
                    recipesDataGrid.Add(recipe);
                }
            }
            else
            {
                errorDataGridRecipe.Visibility = Visibility.Visible;
                recipeDataGrid.Visibility = Visibility.Collapsed;
            }
            CountPercentagesDataGrid();
            recipeDataGrid.ItemsSource = recipesDataGrid;
        }

        private async void FillRecipesComment()
        {
            var statusRecipe = await apiClient.GetRecipesComment(selectedRecipe.id, "recipe");

            for (int i = 0; i < statusRecipe.Length; i++)
            {
                if (statusRecipe[i].statusNameOld == null)
                {
                    statusRecipe[i].mainInfo1 = "Создан";
                    statusRecipe[i].mainInfo3 = "-";
                }
                else
                {
                    statusRecipe[i].mainInfo1 = "Переведена из";
                    statusRecipe[i].mainInfo2 = "в";
                    statusRecipe[i].mainInfo3 = "-";
                }
            }
            statusDataGrid.ItemsSource = statusRecipe;
        }

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

        private void CountPercentagesDataGrid()
        {
            decimal count = 0;
            foreach (var item in recipesDataGrid)
            {
                count += item.percentage;
            }
            valueProgressBar = count;
            valueTextBlock = Convert.ToString(count);
        }

        private void DeleteSelectedStep_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (RecipesDataGrid)recipeDataGrid.SelectedItem;
            int idRecipe;
            if (selectedProduct != null)
            {
                idRecipe = selectedProduct.id;
                recipesDataGrid.Remove(selectedProduct);
                for (global::System.Int32 i = 0; i < recipesDataGrid.Count; i++)
                {
                    recipesDataGrid[i].id = i + 1;
                    recipesDataGrid[i].loadOrder = i + 1;
                }
                recipeDataGrid.ItemsSource = null;  
                recipeDataGrid.ItemsSource = recipesDataGrid;
                CountPercentagesDataGrid();
            }
        }

        private void CreateStep_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelecteCommponet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = (ComboBox)sender;
            var element = cmb.SelectedItem;
        }

        private void rawMterialsName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = (ComboBox)sender;
            //Сделать что бы textblokc показывал код.
        }
    }
}
