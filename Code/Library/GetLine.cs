using System.Windows.Forms;

namespace ExpressHelper1011.Library
{
    public partial class GetLine : Form
    {
        public GetLine(string text = "",bool capital = false,string labelText = "")
        {
            InitializeComponent();
            if (capital) tbLine.CharacterCasing = CharacterCasing.Upper;
            TitleText = text;
            this.LabelText = labelText;
        }
        public string TitleText
        { set { this.Text = value; } }
        public string Value
        {
            get { return tbLine.Text; }
            set { tbLine.Text = value; }
        }
        public string LabelText
        { set { lblLine.Text = value; } }
        public void csLabelText(string value)
        {lblLine.Text = value; }
        public void csTitleText(string value)
        { this.Text = value; } 
 
    }
}
