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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PlantProtectionTechnologist.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>  
    public partial class Authorization : Page
    {
        private CircleAnimator _circleAnimator;
        private DispatcherTimer _timer;
        public Authorization()
        {
            InitializeComponent();


            _circleAnimator = new CircleAnimator(Circle, orbitRadius: 40, centerX: 50, centerY: 50);
            _timer = new DispatcherTimer();

            _timer.Interval = TimeSpan.FromMilliseconds(20);

            _timer.Tick += MoveCircle_Tick;
         
            this.PreviewMouseDown += (sender, e) =>
            {
                if (!(e.OriginalSource is TextBox))
                {
                    Keyboard.ClearFocus();
                }
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ApiClient apiClient = new ApiClient();
          
            _timer.Start();
            parentGrid.Effect = new BlurEffect() { Radius = 5 };
            Loaded.Visibility = Visibility.Visible;
            failedAuthorization.Visibility = Visibility.Hidden;
            parentGrid.IsHitTestVisible = false;
            bool isAuthorization = await apiClient.GetDataAuthorization(login.Text, password.Text);
            if (isAuthorization)
            {
                parentGrid.IsHitTestVisible = true;
                _timer.Stop();
                parentGrid.Effect = new BlurEffect() { Radius = 0 };
                Navigate.mainFrame.Navigate(new Main());
            }
            else
            {
                _timer.Stop();
                parentGrid.Effect = new BlurEffect() { Radius = 0 };
                Loaded.Visibility = Visibility.Collapsed;
                parentGrid.IsHitTestVisible = true;
                failedAuthorization.Visibility = Visibility.Visible;
            }
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox el = (TextBox)sender;
            Point relative = el.TransformToAncestor(parentGrid).Transform(new Point(0, 0));

            if (string.IsNullOrEmpty(el.Text))
            {
                tipPassword.Visibility = Visibility.Visible;
            }
            else
            {
                tipPassword.Visibility = Visibility.Hidden;
            }
        }
        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox el = (TextBox)sender;
            Point relative = el.TransformToAncestor(parentGrid).Transform(new Point(0, 0));

            if (string.IsNullOrEmpty(el.Text))
            {
                tipLogin.Visibility = Visibility.Visible;
            }
            else
            {
                tipLogin.Visibility = Visibility.Hidden;
            }
        }

        private void MoveCircle_Tick(object sender, EventArgs e)
        {
            _circleAnimator.Move();
        }

        
    }
}
