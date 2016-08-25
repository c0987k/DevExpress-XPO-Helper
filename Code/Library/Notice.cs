using System;
using System.Windows.Forms;

namespace ExpressHelper1011.Library
{
    public partial class Notice : Form
    {
        public Notice(string s)
        {
            InitializeComponent();
            label1.Text = s;
        }
        private void Notice_Activated(object sender, EventArgs e)
        {
            label1.Refresh();
        }
   }
}
