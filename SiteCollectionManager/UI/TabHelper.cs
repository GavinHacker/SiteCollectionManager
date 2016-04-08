using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiteCollectionManager.UI
{
    public class GTabPropertyPage : TabPage
    {
        private PropertyGrid mInternalgrid;

        public GTabPropertyPage()
        {
            base.Controls.Clear();
            base.Controls.Add(this.Grid);
            base.Name = "GProps";
            this.Text = "GProps";
            base.UseVisualStyleBackColor = true;
        }

        public GTabPropertyPage(string title, object obj)
            : this()
        {
            this.Text = title;
            this.Grid.SelectedObject = obj;
        }

        public PropertyGrid Grid
        {
            get
            {
                if (this.mInternalgrid == null)
                {
                    this.mInternalgrid = new PropertyGrid();
                    this.mInternalgrid.Dock = DockStyle.Fill;
                }
                return this.mInternalgrid;
            }
        }
    }
}
