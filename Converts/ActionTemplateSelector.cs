using PlantProtectionTechnologist.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PlantProtectionTechnologist.Converts
{
    public class ActionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ActiveTemplate { get; set; }
        public DataTemplate ArchivedTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate DraftTemplate { get; set; }
        public DataTemplate ConsonanceTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ProductDto product)
            {
                switch (product.statusName)
                {
                    case "Активный":
                        return ActiveTemplate;
                    case "Архивирован":
                        return ArchivedTemplate;
                    case "Черновик":
                        return DraftTemplate;
                    case "На согласовании":
                        return ConsonanceTemplate;
                    default:
                        return DefaultTemplate;
                }
            }


            return DefaultTemplate;
        }
    }
}
