using PlantProtectionTechnologist.ApiGetCs;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlantProtectionTechnologist.Pages.RecipesCreate
{
    /// <summary>
    /// Логика взаимодействия для RecipesCreatePage.xaml
    /// </summary>
    public partial class RecipesCreatePage : Page
    {
        public string name { get; set; }
        public string code { get; set; }
        public string? activeRecipeFill { get; set; }
        public string statusColor { get; set; }
        public string statusName { get; set; }

        public RecipesCreatePage(ProductDto selectedProduct)
        {
            InitializeComponent();
            DataContext = this;
            name = selectedProduct.name;
            code = selectedProduct.code;
            activeRecipeFill = selectedProduct.activeRecipeFill;
            statusColor = selectedProduct.statusColor;
            statusName = selectedProduct.statusName;
           
        }
    }
}
