using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ITCompCatalogueApp.Helpers
{
    public class CustomCombox : ComboBox
    {
        private string myValue;
        private bool needsUpdate;

        public override void OnApplyTemplate()
        {
            TextBox tbx = this.GetTemplateChild("PART_EditableTextBox") as TextBox;

            tbx.PreviewKeyDown += (o, e) =>
            {
                this.needsUpdate = true;
            };

            tbx.TextChanged += (o, e) =>
            {
                if (needsUpdate)
                {
                    myValue = tbx.Text;
                    this.needsUpdate = false;
                }
                else
                {
                    tbx.Text = myValue;
                }
            };

            base.OnApplyTemplate();
        }
    }
}
