using PlantProtectionTechnologist.Models;
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

        private string[] _errorEntry = new string[] { "Неверный логин или пароль!", "Ваш аккаунт был заблокирован!" };
        TextBlock errorText;
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
            blurBorder.Effect = new BlurEffect() { Radius = 5 };
            Loaded.Visibility = Visibility.Visible;
            failedAuthorization.Visibility = Visibility.Hidden;
            blurBorder.IsHitTestVisible = false;


            if (errorText != null)
            {
                failedAuthorization.Children.Remove(errorText);                
            }

            errorText = new TextBlock();
            errorText.Foreground = new SolidColorBrush(Colors.Red);
            Grid.SetRow(errorText, 0);

            DataUser? authorization = await apiClient.GetDataAuthorization(login.Text, password.Text);
            if (authorization != null)
            {
                ButtonManager.instance.AddDataUser(authorization);
                if (authorization.isActive == true && authorization.roleId == 1)
                {
                    blurBorder.IsHitTestVisible = true;
                    _timer.Stop();
                    blurBorder.Effect = new BlurEffect() { Radius = 0 };
                    Navigate.mainFrame.Navigate(new Main());
                }
                else if (authorization.isActive == false)
                {
                    _timer.Stop();
                    blurBorder.Effect = new BlurEffect() { Radius = 0 };
                    Loaded.Visibility = Visibility.Collapsed;
                    blurBorder.IsHitTestVisible = true;
                    failedAuthorization.Visibility = Visibility.Visible;
                    failedAuthorization.Children.Add(errorText);
                    errorText.Text = _errorEntry[1];
                }
                else if (authorization.isActive == true && authorization.roleId != 1)
                {
                    _timer.Stop();
                    Loaded.Visibility = Visibility.Collapsed;
                    confirmationCard.Visibility = Visibility.Visible;
                }
            }
            else
            {
                _timer.Stop();
                blurBorder.Effect = new BlurEffect() { Radius = 0 };
                Loaded.Visibility = Visibility.Collapsed;
                blurBorder.IsHitTestVisible = true;
                failedAuthorization.Visibility = Visibility.Visible;
                failedAuthorization.Children.Add(errorText);
                errorText.Text = _errorEntry[0];
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

        private void ConfirmationEntry_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn != null)
            {
                int tag = Convert.ToInt32(btn.Tag.ToString());

                if (tag == 0)
                {
                    confirmationCard.Visibility = Visibility.Collapsed;
                    blurBorder.Effect = new BlurEffect() { Radius = 0 };
                    blurBorder.IsHitTestVisible = true;
                }
                else if (tag == 1)
                {
                    Navigate.mainFrame.Navigate(new MainTab(true));                   
                }               
            }
        }
    }
}
