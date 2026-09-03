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

namespace PlantProtectionTechnologist.Pages.ProductActions
{
    /// <summary>
    /// Логика взаимодействия для Viewing.xaml
    /// </summary>
    public partial class Viewing : Page
    {          
        public Viewing(ProductDto selectedProduct)
        {
            InitializeComponent();
            DataContext = selectedProduct;
        }
        private void CloseAddProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Navigate.tabFrame.Navigate(new Production());
        }
    }
}
