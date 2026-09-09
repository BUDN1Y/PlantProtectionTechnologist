using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PlantProtectionTechnologist.Converts
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string colorString && !string.IsNullOrEmpty(colorString))
            {
                try
                {
                    return new BrushConverter().ConvertFromString(colorString);
                }
                catch 
                {
                    return Brushes.Gray;
                }
            }
            return Brushes.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
