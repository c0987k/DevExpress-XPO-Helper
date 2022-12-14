using ExpressHelper1011.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace ExpressHelper2014.MainWindow
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private List<string> classes;
        private void Main_Load(object sender, EventArgs e)
        {
            // MyLib.SetUpTextboxLabels(this.Controls);
            classes = new List<string>()
                          {
                              "Guid",
                              "int",
                              "string",
                              "double",
                              "decimal",
                              "DateTime",
                              "bool"
                          };
            classes.Sort();
            cbPropertyType.csSetComboList<string>(classes);
            MyLib.SetButtonsGreen(this.Controls);
            SetVersion();
            rbIndex = rbProperty.Tag.ToString().csToInteger();

        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string s = string.Empty;
            string propertyName = tbPropertyNameOne.Text.ToLower();
            propertyName = ToTitleCase(propertyName);
            if (propertyName.Contains(" "))
            {
                propertyName = propertyName.Replace(" ", "");
            }
            var two = tbPropertyNameTwo.Text;
            if (two.Contains(" "))
            {
                two = two.Replace(" ", "");
            }
            tbPropertyNameTwo.Text = two;
            if (cbPropertyType.SelectedIndex == -1)
            {
                classes.Add(cbPropertyType.Text);
                classes.Sort();
                var t = cbPropertyType.Text;
                cbPropertyType.csSetComboList<string>(classes);
                cbPropertyType.csSetComboBox<string>(t);
            }
            if (gbMaxLength.Visible)
            {
                s =
                     @"private {0} _{2};
[Size({3})]
public {0} {1}
                    {{
                        get=> _{2}; 
                        set => SetPropertyValue(""{1}"", ref _{2}, value.csLeft({3}));
                    }}
";
                s = string.Format(s, cbPropertyType.Text, propertyName, propertyName.ToLower(), tbMaxLength.Text);
                tbDefinationOne.Text = s;
                Clipboard.SetData(DataFormats.Text, tbDefinationOne.Text);
            }
            else
            {
                s =
                   @"private {0} _{2};
public {0} {1}
                    {{
                        get => _{2}; 
                        set => SetPropertyValue(""{1}"", ref _{2}, value); 
                    }}
";
                s = string.Format(s, cbPropertyType.Text, propertyName, propertyName.ToLower());
                tbDefinationOne.Text = s;
                Clipboard.SetData(DataFormats.Text, tbDefinationOne.Text);
            }
            //            switch (rbIndex)
            //            {
            //                case 1: // single
            //                    if (gbMaxLength.Visible)
            //                    {
            //                        s =
            //                             @"private {0} _{2};
            //[Size({3})]
            //public {0} {1}
            //                    {{
            //                        get=> _{2}; 
            //                        set => SetPropertyValue(""{1}"", ref _{2}, value.csLeft({3}));
            //                    }}
            //";
            //                        s = string.Format(s, cbPropertyType.Text, tbPropertyNameOne.Text, tbPropertyNameOne.Text.ToLower(), tbMaxLength.Text);
            //                        tbDefinationOne.Text = s;
            //                        Clipboard.SetData(DataFormats.Text, tbDefinationOne.Text);
            //                    }
            //                    else
            //                    {
            //                        s =
            //                           @"private {0} _{2};
            //public {0} {1}
            //                    {{
            //                        get => _{2}; 
            //                        set => SetPropertyValue(""{1}"", ref _{2}, value); 
            //                    }}
            //";
            //                        s = string.Format(s, cbPropertyType.Text, tbPropertyNameOne.Text, tbPropertyNameOne.Text.ToLower());
            //                        tbDefinationOne.Text = s;
            //                        Clipboard.SetData(DataFormats.Text, tbDefinationOne.Text);
            //                    }
            //                    break;
            //                ///////////////////////////////////////////////////////////////////////////////
            //                /// //////////////////////////////////////////////////////////////////////////
            //                case 2: // one to one

            //                    s =
            //                              @"private {0} _{2};
            //[Association(""{3}"",typeof({4}))]
            //public {0} {1}
            //                    {{
            //                        get => _{2}; 
            //                        set => SetPropertyValue(""{1}"", ref _{2}, value);
            //                    }}
            //";
            //                    s = string.Format(s, cbPropertyType.Text, tbPropertyNameOne.Text, tbPropertyNameOne.Text.ToLower(), tbRelationshipName.Text, cbPropertyType.Text);
            //                    tbDefinationOne.Text = s;
            //                    s =
            //                              @"private {0} _{2};
            //[Association(""{3}"",typeof({4}))]
            //public {0} {1}
            //                    {{
            //                        get => _{2}; }}
            //                        set => SetPropertyValue(""{1}"", ref _{2}, value); 
            //                    }}
            //";
            //                    s = string.Format(s, tbClassTwo.Text, tbPropertyNameTwo.Text, tbPropertyNameTwo.Text.ToLower(), tbRelationshipName.Text, tbClassTwo.Text);

            //                    tbDefinationMany.Text = s;
            //                    break;
            //                //////////////////////////////////////////////////////////////////////////////
            //                /// //////////////////////////////////////////////////////////////////////////
            //                case 3: // one to many
            //                    // many TAsset_Status
            //                    s =
            //                    @"[Association(""{0}"")]
            //        public XPCollection<{2}> {3} 
            //{{ 
            //get => GetCollection<{4}>(""{5}""); 
            //}}
            //";

            //                    s = string.Format(s
            //                        , tbRelationshipName.Text //0
            //                        , cbPropertyType.Text  //1
            //                        , cbPropertyType.Text  //2
            //                        , tbPropertyNameOne.Text  //3
            //                        , cbPropertyType.Text  //4
            //                        , tbPropertyNameOne.Text //5
            //                        );
            //                    tbDefinationOne.Text = s;
            //                    // employee
            //                    s = @"private {2} _{6}; 
            //[Association(""{0}"")]
            //public {2} {5}
            //                    {{
            //                        get => _{6}; 
            //                        set => SetPropertyValue(""{5}"", ref _{6}, value); 
            //                    }}
            //";
            //                    s = string.Format(s
            //                             , tbRelationshipName.Text //0
            //                             , cbPropertyType.Text //1
            //                             , tbClassTwo.Text //2
            //                             , tbPropertyNameOne.Text //3
            //                             , tbPropertyNameOne.Text.ToLower() //4
            //                             , tbPropertyNameTwo.Text //5
            //                             , tbPropertyNameTwo.Text.ToLower() //6
            //                             ); //5   
            //                    tbDefinationMany.Text = s;


            //                    break;

            //                case 4: // many to many
            //                    s =
            //                           @"[Association(""{0}"")]
            //        public XPCollection<{2}> {3} 
            //{{
            //get => GetCollection<{4}>(""{5}""); 
            //}}
            //";

            //                    s = string.Format(s
            //                        , tbRelationshipName.Text //0
            //                        , cbPropertyType.Text  //1
            //                        , cbPropertyType.Text  //2
            //                        , tbPropertyNameOne.Text  //3
            //                        , cbPropertyType.Text  //4
            //                        , tbPropertyNameOne.Text //5
            //                        );
            //                    tbDefinationOne.Text = s;


            //                    s = @"[Association(""{0}"")]
            //        public XPCollection<{2}> {3} 
            //{{ 
            //get => GetCollection<{4}>(""{5}""); 
            //}}
            //";

            //                    s = string.Format(s
            //                       , tbRelationshipName.Text //0
            //                       , tbClassTwo.Text  //1
            //                       , tbClassTwo.Text  //2
            //                       , tbPropertyNameTwo.Text  //3
            //                       , tbClassTwo.Text  //4
            //                       , tbPropertyNameTwo.Text //5
            //                       );
            //                    tbDefinationMany.Text = s;
            //                    break;
            //                default:
            //                    return;
            //                    break;
            //            }
        }

        private string Version()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.Substring(0, 6);
        }
        private void SetVersion()
        {
            this.Text = Version();
        }
        private string ToTitleCase(string text)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(text);
        }

        private void btnCopy1_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, tbDefinationOne.Text);
        }

        private void btnCopy2_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, tbDefinationMany.Text);
        }

        private void rbOne2One_Click(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null) return;
            rbIndex = rb.Tag.ToString().csToInteger();
            tbRelationshipName.Focus();
        }
        private int rbIndex = 3;
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            string class1 = cbPropertyType.Text;
            string class2 = tbClassTwo.Text;
            string property1 = tbPropertyNameOne.Text;
            string property2 = tbPropertyNameTwo.Text;
            cbPropertyType.Text = class2;
            tbClassTwo.Text = class1;
            tbPropertyNameOne.Text = property2;
            tbPropertyNameTwo.Text = property1;
        }

        private void tbPropertyNameOne_Enter(object sender, EventArgs e)
        {
            tbPropertyNameOne.Clear();
            btnGenerateClass.csSetColor();
        }

        private void btnGenerateClass_Click(object sender, EventArgs e)
        {
            btnGenerateClass.BackColor = Color.Red;
            var classString = string.Format(@"public class {0} : XPObject
    {{
        public {0}(Session session)
            : base(session)
        {{
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }}
       
    }}", tbClassName.Text);
            Clipboard.SetText(classString); //   .SetData("OemText",classString);
            tbClass.Text = classString;
        }

        private void cbClassOne_TextChanged_1(object sender, EventArgs e)
        {
            gbMaxLength.Visible = cbPropertyType.Text == @"string";
        }

        private void toggleOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            toggleOnTopToolStripMenuItem.BackColor = this.TopMost ? Color.Red : Color.White;
            MoveCursor(new Point(100, 10));

        }
        private void MoveCursor(Point location)
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(this.Location.X + location.X, this.Location.Y + Location.Y);
            //          Cursor.Clip = new Rectangle(this.Location, this.Size);
        }

    }
}
