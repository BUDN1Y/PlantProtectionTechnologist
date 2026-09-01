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
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace PlantProtectionTechnologist
{
    public class VisualAddEditProduct : INotifyPropertyChanged
    {
        private List<ProductDto> _dataProduction;
        string[] typeProductName = new string[] { "Гербицид", "Инсектицид", "Фунгицид", "Регулятор роста", "Протравитель" };
        string[] releaseFormProductName = new string[] { "Эмульсия", "Порошок", "Суспензия", "Гранулы" };
        public readonly (string prefixNameAbbreviation, string? prefixNameDecoding)[] prefixName;

        public string textPrefix;

        private string? _confirmationTypeProduct;
        public string? confirmationTypeProduct { get => _confirmationTypeProduct; set { _confirmationTypeProduct = value; OnpropertyChanget(nameof(confirmationTypeProduct)); } }

        private string? _confirmationReleaseForm;
        public string? confirmationReleaseForm { get => _confirmationReleaseForm; set { _confirmationReleaseForm = value; OnpropertyChanget(nameof(confirmationReleaseForm)); } }

        private string? _confirmationComment;
        public string? confirmationComment { get => _confirmationComment; set { _confirmationComment = value; OnpropertyChanget(nameof(confirmationComment)); } }

        private string? _textName;
        public string? textName { get => _textName; set { _textName = value; OnpropertyChanget(nameof(textName)); } }

        private string _textCode;
        public string textCode { get => _textCode; set { _textCode = value; OnpropertyChanget(nameof(textCode)); } }

        private string _confirmationStatus;
        public string confirmationStatus { get => _confirmationStatus; set { _confirmationStatus = value; OnpropertyChanget(nameof(confirmationStatus)); } }

        private ComboBox _releaseFormProduct;
        private ComboBox _typeProduct;

        public VisualAddEditProduct(List<ProductDto> dataProduction, ComboBox typeProduct, ComboBox releaseFormProduct)
        {
            this._dataProduction = dataProduction;
            _typeProduct = typeProduct;
            _releaseFormProduct = releaseFormProduct; 

            prefixName = new (string, string?)[]
            {
               ("N-", null),
               ("HERB-", "Гербицид"),
               ("INSECT-", "Инсектицид"),
               ("FUNG-", "Фунгицид"),
               ("REG-", "Регулятор роста"),
               ("TREAT-", "Протравитель"),
            };

            ComboBoxItem itemtypeProduct = new ComboBoxItem();
            itemtypeProduct.Content = "Выберите тип...";
            itemtypeProduct.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999"));
            itemtypeProduct.IsSelected = true;
            typeProduct.Items.Add(itemtypeProduct);

            foreach (var item in typeProductName)
            {
                typeProduct.Items.Add(item);
            }

            ComboBoxItem itemReleaseForm = new ComboBoxItem();
            itemReleaseForm.Content = "Выберите форму...";
            itemReleaseForm.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999"));
            itemReleaseForm.IsSelected = true;
            releaseFormProduct.Items.Add(itemReleaseForm);

            foreach (var item in releaseFormProductName)
            {
                releaseFormProduct.Items.Add(item);
            }

            SelectTextCode(0);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnpropertyChanget(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void typeProduct_SelectionChanged()
        {
            ComboBox comboBox = _typeProduct;

            int selectedIndex = comboBox.SelectedIndex;

            confirmationTypeProduct = prefixName[selectedIndex].prefixNameDecoding;

            SelectTextCode(selectedIndex);
        }

        public void releaseFormProduct_SelectionChanged()
        {
            ComboBox comboBox = _releaseFormProduct;

            int selectedIndex = comboBox.SelectedIndex;

            confirmationReleaseForm = (selectedIndex == 0) ? null : comboBox.SelectedItem.ToString();
        }

        private void SelectTextCode(int selectedIndex)
        {
            textPrefix = prefixName[selectedIndex].prefixNameAbbreviation;
            string countPrefix = (_dataProduction.Count(x => x.code.Contains(textPrefix)) + 1).ToString();

            if (countPrefix.Length == 1)
            {
                countPrefix = AddZeros(countPrefix, 2);
            }
            else if (countPrefix.Length == 2)
            {
                countPrefix = AddZeros(countPrefix, 1);
            }

            textCode = $"{textPrefix}{countPrefix}";

        }

        private string AddZeros(string countPrefix, int Add)
        {
            string result = "";
            for (int i = 0; i < Add; i++)
            {
                result += "0";
            }
            result += countPrefix;
            return result;
        }
        
    }
}
