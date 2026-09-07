using DocumentFormat.OpenXml.Drawing;
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

namespace PlantProtectionTechnologist.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainTab.xaml
    /// </summary>
    public partial class MainTab : Page
    {
        ButtonManager instance = ButtonManager.instance;
        private bool _isRole = false;
        public MainTab()
        {
            InitializeComponent();

        }

        public MainTab(bool isRole)
        {
            InitializeComponent();
            _isRole = isRole;
        }
   
        private void PageSwitch_Click(object sender, MouseButtonEventArgs e)
        {
            if (_isRole)
            {
                mainBorder.Effect = new BlurEffect() { Radius = 15};
                mainBorder.IsHitTestVisible = false;
                confirmationCard.Visibility = Visibility.Visible;
                return;
            }

            var btn = sender as TextBlock;
            
            if (btn != null)
            {
                string? tag = btn.Tag.ToString();
                var indexNextPage = Convert.ToInt32(tag);
                
                instance.ClearSelectedButton();
                instance.AddSelectedButton(indexNextPage);

                switch (indexNextPage)
                {
                    case 0:
                        Navigate.tabFrame.Navigate(new MainTab());
                        break;
                    case 1:
                        Navigate.tabFrame.Navigate(new Production());
                        break;
                    case 2:
                        Navigate.tabFrame.Navigate(new Recipes());
                        break;
                    case 3:
                        Navigate.tabFrame.Navigate(new MainTab());
                        break;
                    case 4:
                        Navigate.tabFrame.Navigate(new MainTab());
                        break;
                    case 5:
                        Navigate.tabFrame.Navigate(new MainTab());
                        break;
                }
            }
        }

        private void Confirmation_Click(object sender, RoutedEventArgs e)
        {
            mainBorder.Effect = new BlurEffect() { Radius = 0 };
            mainBorder.IsHitTestVisible = true;
            confirmationCard.Visibility = Visibility.Collapsed;
        }
    }
}
