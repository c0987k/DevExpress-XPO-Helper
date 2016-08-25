using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ExpressHelper1011.Library
{
    public partial class GetFromDropDown : Form
    {
        public GetFromDropDown()
        {
            InitializeComponent();
            Value = cbEdit;
            Text = "Select Street";
        }
        public string TitleText
        { set { this.Text = value; } }
        public ComboBoxEdit Value { get; set; }
        public string LabelText
        { set { lblLine.Text = value; } }
        public void csLabelText(string value)
        { lblLine.Text = value; }
        public void csTitleText(string value)
        { this.Text = value; }
    }
}
