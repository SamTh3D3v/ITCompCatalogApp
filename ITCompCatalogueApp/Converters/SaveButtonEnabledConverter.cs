using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ITCompCatalogueApp.Converters
{
    class SaveButtonEnabledConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null) return false;
            var cour = (value as Cour);
            if(!String.IsNullOrEmpty(cour.Code) &&
                !String.IsNullOrEmpty(cour.Intitule) &&
                    !String.IsNullOrEmpty(cour.Duree.ToString(CultureInfo.InvariantCulture)) &&
                cour.Category !=null &&
                !String.IsNullOrEmpty(cour.Annee.ToString(CultureInfo.InvariantCulture)))
            return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
