using System;
using System.Windows.Forms;

namespace Athame.UI
{
    public partial class PathFormatHelpForm : AthameDialog
    {
        public PathFormatHelpForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
