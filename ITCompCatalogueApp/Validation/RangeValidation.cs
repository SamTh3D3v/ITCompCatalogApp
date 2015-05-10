using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ITCompCatalogueApp.Validation
{
    public class RangeValidation : ValidationRule
    {
        #region Properties
        public int Min { get; set; }
        public int Max { get; set; }
        #endregion
        #region Ctrs and Methods 
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val = 0;

            try
            {
                var s = (string)value;
                if (!string.IsNullOrEmpty(s))
                    val = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Caractere invalide " + e.Message);
            }

            if (Min != -1)
            {
                if ((val < Min) || (val > Max))
                {
                    return new ValidationResult(false,
                        "cette valeur doit etre dans le rang: " + Min + " - " + Max + ".");
                }
                else
                {
                    return new ValidationResult(true, null);
                }
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
        #endregion       
    }
}
