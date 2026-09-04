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

namespace PlantProtectionTechnologist.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page, INotifyPropertyChanged
    {
        Grid[] allGridButton = new Grid[9];
        string[] nameGrid = new string[] { "mainTab", "production", "recipes", "technologicalMaps", "productionOrders", "productionBatches", "extruderPrograms", "deviationsEvents", "reports" };
        
        private string _tipText1;
        public string tipText1 { get => _tipText1; set { _tipText1 = value; OnpropertyChanget(nameof(tipText1)); } }

        private string _tipText2;
        public string tipText2 { get => _tipText2; set { _tipText2 = value; OnpropertyChanget(nameof(tipText2)); } }

        private string _tipImage;
        public string tipImage { get => _tipImage; set { _tipImage = value; OnpropertyChanget(nameof(tipImage)); } }
        public event PropertyChangedEventHandler? PropertyChanged;
        ButtonManager instance = ButtonManager.instance;
        public Main()
        {
            InitializeComponent();

            
            DataContext = instance;
            

            Navigate.tabFrame = tabFrame;
            Navigate.tabFrame.Navigate(new MainTab());

            for (int i = 0; i < 9; i++)
            {
                allGridButton[i] = (Grid)mainGrid.FindName(nameGrid[i]);
            }
            instance.RegisterAllGridButton(allGridButton);
            instance.AddSelectedButton(0);

        }
        protected void OnpropertyChanget(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private void PageSwitch_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
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

        private void ConfirmationClose_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn != null)
            {
                int tag = Convert.ToInt32(btn.Tag.ToString());

                if (tag == 0)
                {
                    confirmationCard.Visibility = Visibility.Collapsed;
                    mainGrid.IsHitTestVisible = true;
                    borderBlur.Effect = new BlurEffect() { Radius = 0 };
                }
                else if(tag == 1)
                {
                    confirmationCard.Visibility = Visibility.Visible;
                    mainGrid.IsHitTestVisible = false;
                    borderBlur.Effect = new BlurEffect() { Radius = 15 };
                }
                else
                {
                    Navigate.mainFrame.Navigate(new Authorization());
                }
            }
        }

        
    }
}
