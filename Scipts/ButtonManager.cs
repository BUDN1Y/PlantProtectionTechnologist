using PlantProtectionTechnologist.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PlantProtectionTechnologist.Scipts
{
    public sealed class ButtonManager : INotifyPropertyChanged
    {
        private static ButtonManager _instance;
        private ButtonManager() { }

        public static ButtonManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ButtonManager();
                }
                return _instance;
            }
        }

        string[,] nameTipText = new string[,]
        {
            {"Главная панель управления", " | Обзор производства", "Home" },
            {"Продукция", " | Справочник выпускаемой продукции", "PackageVariant" },
            {"Рецептуры", " | Управление рецептурами", "ListBox" },
            {"Технологические карты", " | Управление техкартами", "Gear" },
            {"Производственные заказы", " | Управление заказами", "FileDocument" },
            {"Производственные партии", " | Управление партиями", "Factory" },
            {"Программы экструдера", " | Управление программами", "Tune" },
            {"Отклонения и события", " | Мониторинг событий", "Alert" },
            {"Отчеты", " | Генерация отчетов", "ChartBar" }
        };

        public DataUser user;
        private Grid[] _allGridButton = new Grid[9];
        private int _indexGrid = 0;

        private string _fio;
        public string fio { get => _fio; set { _fio = value; OnpropertyChanget(nameof(fio)); } }

        private string? _departmentsName;
        public string? departmentsName { get => _departmentsName; set { _departmentsName = value; OnpropertyChanget(nameof(departmentsName)); } }

        private string? _departmentsDescription;
        public string? departmentsDescription { get => _departmentsDescription; set { _departmentsDescription = value; OnpropertyChanget(nameof(departmentsDescription)); } }

        private string _tipText1;
        public string tipText1 { get => _tipText1; set { _tipText1 = value; OnpropertyChanget(nameof(tipText1)); } }

        private string _tipText2;
        public string tipText2 { get => _tipText2; set { _tipText2 = value; OnpropertyChanget(nameof(tipText2)); } }

        private string _tipImage;
        public string tipImage { get => _tipImage; set { _tipImage = value; OnpropertyChanget(nameof(tipImage)); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnpropertyChanget(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RegisterAllGridButton(Grid[] allGridButton)
        {
            _allGridButton = allGridButton.ToArray();
        }

        public void AddDataUser(DataUser dataUser)
        {
            user = dataUser;
            fio = dataUser.fullName;
            departmentsDescription = dataUser.departmentDescription;
            departmentsName = dataUser.departmentName;
        }
        public void ClearSelectedButton()
        {
            _allGridButton[_indexGrid].Background.Opacity = 0;
            foreach (var item in _allGridButton[_indexGrid].Children)
            {
                if (item is Rectangle rect)
                {
                    rect.Visibility = Visibility.Hidden;
                }
            }
        }

        public void AddSelectedButton(int indexButton)
        {
            tipText1 = nameTipText[indexButton, 0];
            tipText2 = nameTipText[indexButton, 1];
            tipImage = nameTipText[indexButton, 2];
            _allGridButton[indexButton].Background.Opacity = 0.25;
            foreach (var item in _allGridButton[indexButton].Children)
            {
                if (item is Rectangle rect)
                {
                    rect.Visibility = Visibility.Visible;
                }
            }
            _indexGrid = indexButton;
        }
    }
}
