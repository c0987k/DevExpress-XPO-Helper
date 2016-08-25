using System;
using System.Windows.Forms;

namespace ExpressHelper1011.Library
{
    public partial class GetBlockText : Form
    {
        public GetBlockText(string text = "", string labelText = "Enter Comment")
        {
            InitializeComponent();
            TitleText = text;
            this.LabelText = labelText;
            spellChecker1.csLoadSpellCheckDictionaries();
        }

        public string TitleText
        { set { this.Text = value; } }
        public string Value
        {
            get { return tbText.Text; }
            set { tbText.Text = value; }
        }
        public string LabelText
        { set { lblLine.Text = value; } }
        public void csLabelText(string value)
        { lblLine.Text = value; }
        public void csTitleText(string value)
        { this.Text = value; }
        private void GetBlockText_Load(object sender, EventArgs e)
        {
            spellChecker1.csLoadSpellCheckDictionaries();
            tbText.csSpellCheck(spellChecker1);
        }
    }
}
