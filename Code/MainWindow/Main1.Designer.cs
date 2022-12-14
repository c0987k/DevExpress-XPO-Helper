namespace ExpressHelper2014.MainWindow
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling1 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling2 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling3 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling4 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling5 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling6 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling7 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling8 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling9 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling10 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnGenerate = new System.Windows.Forms.Button();
            this.tbDefinationOne = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbPropertyNameTwo = new System.Windows.Forms.TextBox();
            this.tbPropertyNameOne = new System.Windows.Forms.TextBox();
            this.tbDefinationMany = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbOne2Many = new System.Windows.Forms.RadioButton();
            this.rbProperty = new System.Windows.Forms.RadioButton();
            this.rbOne2One = new System.Windows.Forms.RadioButton();
            this.rbMany2Many = new System.Windows.Forms.RadioButton();
            this.btnCopy2 = new System.Windows.Forms.Button();
            this.tbClassTwo = new System.Windows.Forms.TextBox();
            this.tbRelationshipName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCopy1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckbDropFirstLetter = new System.Windows.Forms.CheckBox();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.cbPropertyType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnGenerateClass = new System.Windows.Forms.Button();
            this.tbClassName = new System.Windows.Forms.TextBox();
            this.tbClass = new System.Windows.Forms.TextBox();
            this.spellChecker1 = new DevExpress.XtraSpellChecker.SpellChecker(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbMaxLength = new System.Windows.Forms.TextBox();
            this.gbMaxLength = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbPropertyType.Properties)).BeginInit();
            this.gbMaxLength.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 218);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(109, 34);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // tbDefinationOne
            // 
            this.tbDefinationOne.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDefinationOne.Location = new System.Drawing.Point(12, 258);
            this.tbDefinationOne.Multiline = true;
            this.tbDefinationOne.Name = "tbDefinationOne";
            this.tbDefinationOne.Size = new System.Drawing.Size(530, 89);
            this.spellChecker1.SetSpellCheckerOptions(this.tbDefinationOne, optionsSpelling1);
            this.tbDefinationOne.TabIndex = 2;
            this.tbDefinationOne.Text = "Defination";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem,
            this.toggleOnTopToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(548, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::ExpressHelper2014.Properties.Resources.Ribbon_Exit_32x32;
            this.closeToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toggleOnTopToolStripMenuItem
            // 
            this.toggleOnTopToolStripMenuItem.Name = "toggleOnTopToolStripMenuItem";
            this.toggleOnTopToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.toggleOnTopToolStripMenuItem.Text = "Toggle On Top";
            this.toggleOnTopToolStripMenuItem.Click += new System.EventHandler(this.toggleOnTopToolStripMenuItem_Click);
            // 
            // tbPropertyNameTwo
            // 
            this.tbPropertyNameTwo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbPropertyNameTwo.Location = new System.Drawing.Point(328, 134);
            this.tbPropertyNameTwo.Name = "tbPropertyNameTwo";
            this.tbPropertyNameTwo.Size = new System.Drawing.Size(132, 20);
            this.spellChecker1.SetSpellCheckerOptions(this.tbPropertyNameTwo, optionsSpelling2);
            this.tbPropertyNameTwo.TabIndex = 6;
            this.tbPropertyNameTwo.Text = "PROPERTY NAME";
            // 
            // tbPropertyNameOne
            // 
            this.tbPropertyNameOne.AllowDrop = true;
            this.tbPropertyNameOne.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbPropertyNameOne.Location = new System.Drawing.Point(133, 134);
            this.tbPropertyNameOne.Name = "tbPropertyNameOne";
            this.tbPropertyNameOne.Size = new System.Drawing.Size(140, 20);
            this.spellChecker1.SetSpellCheckerOptions(this.tbPropertyNameOne, optionsSpelling3);
            this.tbPropertyNameOne.TabIndex = 11;
            this.tbPropertyNameOne.Text = "PROPERTY NAME";
            this.tbPropertyNameOne.Enter += new System.EventHandler(this.tbPropertyNameOne_Enter);
            // 
            // tbDefinationMany
            // 
            this.tbDefinationMany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDefinationMany.Location = new System.Drawing.Point(12, 382);
            this.tbDefinationMany.Multiline = true;
            this.tbDefinationMany.Name = "tbDefinationMany";
            this.tbDefinationMany.Size = new System.Drawing.Size(530, 88);
            this.spellChecker1.SetSpellCheckerOptions(this.tbDefinationMany, optionsSpelling4);
            this.tbDefinationMany.TabIndex = 13;
            this.tbDefinationMany.Text = "Defination";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOne2Many);
            this.groupBox1.Controls.Add(this.rbProperty);
            this.groupBox1.Controls.Add(this.rbOne2One);
            this.groupBox1.Controls.Add(this.rbMany2Many);
            this.groupBox1.Location = new System.Drawing.Point(12, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(109, 121);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Relationship";
            // 
            // rbOne2Many
            // 
            this.rbOne2Many.AutoSize = true;
            this.rbOne2Many.Location = new System.Drawing.Point(10, 69);
            this.rbOne2Many.Name = "rbOne2Many";
            this.rbOne2Many.Size = new System.Drawing.Size(83, 17);
            this.rbOne2Many.TabIndex = 13;
            this.rbOne2Many.Tag = "3";
            this.rbOne2Many.Text = "One 2 Many";
            this.rbOne2Many.UseVisualStyleBackColor = true;
            this.rbOne2Many.Click += new System.EventHandler(this.rbOne2One_Click);
            // 
            // rbProperty
            // 
            this.rbProperty.AutoSize = true;
            this.rbProperty.Checked = true;
            this.rbProperty.Location = new System.Drawing.Point(10, 19);
            this.rbProperty.Name = "rbProperty";
            this.rbProperty.Size = new System.Drawing.Size(64, 17);
            this.rbProperty.TabIndex = 12;
            this.rbProperty.TabStop = true;
            this.rbProperty.Tag = "1";
            this.rbProperty.Text = "Property";
            this.rbProperty.UseVisualStyleBackColor = true;
            this.rbProperty.Click += new System.EventHandler(this.rbOne2One_Click);
            // 
            // rbOne2One
            // 
            this.rbOne2One.AutoSize = true;
            this.rbOne2One.Location = new System.Drawing.Point(10, 44);
            this.rbOne2One.Name = "rbOne2One";
            this.rbOne2One.Size = new System.Drawing.Size(77, 17);
            this.rbOne2One.TabIndex = 8;
            this.rbOne2One.Tag = "2";
            this.rbOne2One.Text = "One 2 One";
            this.rbOne2One.UseVisualStyleBackColor = true;
            this.rbOne2One.Click += new System.EventHandler(this.rbOne2One_Click);
            // 
            // rbMany2Many
            // 
            this.rbMany2Many.AutoSize = true;
            this.rbMany2Many.Location = new System.Drawing.Point(10, 94);
            this.rbMany2Many.Name = "rbMany2Many";
            this.rbMany2Many.Size = new System.Drawing.Size(89, 17);
            this.rbMany2Many.TabIndex = 9;
            this.rbMany2Many.Tag = "4";
            this.rbMany2Many.Text = "Many 2 Many";
            this.rbMany2Many.UseVisualStyleBackColor = true;
            this.rbMany2Many.Click += new System.EventHandler(this.rbOne2One_Click);
            // 
            // btnCopy2
            // 
            this.btnCopy2.Location = new System.Drawing.Point(133, 353);
            this.btnCopy2.Name = "btnCopy2";
            this.btnCopy2.Size = new System.Drawing.Size(75, 23);
            this.btnCopy2.TabIndex = 18;
            this.btnCopy2.Text = "Copy";
            this.btnCopy2.UseVisualStyleBackColor = true;
            this.btnCopy2.Click += new System.EventHandler(this.btnCopy2_Click);
            // 
            // tbClassTwo
            // 
            this.tbClassTwo.Location = new System.Drawing.Point(328, 60);
            this.tbClassTwo.Name = "tbClassTwo";
            this.tbClassTwo.Size = new System.Drawing.Size(107, 20);
            this.spellChecker1.SetSpellCheckerOptions(this.tbClassTwo, optionsSpelling5);
            this.tbClassTwo.TabIndex = 19;
            this.tbClassTwo.Text = "TAsset";
            // 
            // tbRelationshipName
            // 
            this.tbRelationshipName.Location = new System.Drawing.Point(169, 95);
            this.tbRelationshipName.Name = "tbRelationshipName";
            this.tbRelationshipName.Size = new System.Drawing.Size(205, 20);
            this.spellChecker1.SetSpellCheckerOptions(this.tbRelationshipName, optionsSpelling6);
            this.tbRelationshipName.TabIndex = 21;
            this.tbRelationshipName.Text = "Relationship Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Class";
            // 
            // btnCopy1
            // 
            this.btnCopy1.AccessibleRole = System.Windows.Forms.AccessibleRole.WhiteSpace;
            this.btnCopy1.Location = new System.Drawing.Point(133, 224);
            this.btnCopy1.Name = "btnCopy1";
            this.btnCopy1.Size = new System.Drawing.Size(75, 23);
            this.btnCopy1.TabIndex = 17;
            this.btnCopy1.Text = "Copy";
            this.btnCopy1.UseVisualStyleBackColor = true;
            this.btnCopy1.Click += new System.EventHandler(this.btnCopy1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Property Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(328, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Property Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckbDropFirstLetter);
            this.groupBox2.Location = new System.Drawing.Point(242, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(115, 36);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Property Names";
            // 
            // ckbDropFirstLetter
            // 
            this.ckbDropFirstLetter.AutoSize = true;
            this.ckbDropFirstLetter.Checked = true;
            this.ckbDropFirstLetter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbDropFirstLetter.Location = new System.Drawing.Point(8, 13);
            this.ckbDropFirstLetter.Name = "ckbDropFirstLetter";
            this.ckbDropFirstLetter.Size = new System.Drawing.Size(101, 17);
            this.ckbDropFirstLetter.TabIndex = 0;
            this.ckbDropFirstLetter.Text = "Drop First Letter";
            this.ckbDropFirstLetter.UseVisualStyleBackColor = true;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Location = new System.Drawing.Point(239, 59);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(80, 23);
            this.btnSwitch.TabIndex = 28;
            this.btnSwitch.Text = "<= Swap =>";
            this.btnSwitch.UseVisualStyleBackColor = true;
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // cbPropertyType
            // 
            this.cbPropertyType.Location = new System.Drawing.Point(133, 60);
            this.cbPropertyType.Name = "cbPropertyType";
            this.cbPropertyType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbPropertyType.Size = new System.Drawing.Size(100, 20);
            this.cbPropertyType.TabIndex = 29;
            this.cbPropertyType.TextChanged += new System.EventHandler(this.cbClassOne_TextChanged_1);
            // 
            // btnGenerateClass
            // 
            this.btnGenerateClass.Location = new System.Drawing.Point(371, 213);
            this.btnGenerateClass.Name = "btnGenerateClass";
            this.btnGenerateClass.Size = new System.Drawing.Size(132, 34);
            this.btnGenerateClass.TabIndex = 30;
            this.btnGenerateClass.Text = "Generate Class";
            this.btnGenerateClass.UseVisualStyleBackColor = true;
            this.btnGenerateClass.Click += new System.EventHandler(this.btnGenerateClass_Click);
            // 
            // tbClassName
            // 
            this.tbClassName.AllowDrop = true;
            this.tbClassName.Location = new System.Drawing.Point(371, 187);
            this.tbClassName.Name = "tbClassName";
            this.tbClassName.Size = new System.Drawing.Size(132, 20);
            this.spellChecker1.SetSpellCheckerOptions(this.tbClassName, optionsSpelling7);
            this.tbClassName.TabIndex = 31;
            this.tbClassName.Text = "Class Name";
            // 
            // tbClass
            // 
            this.tbClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClass.Location = new System.Drawing.Point(11, 476);
            this.tbClass.Multiline = true;
            this.tbClass.Name = "tbClass";
            this.tbClass.Size = new System.Drawing.Size(531, 108);
            this.spellChecker1.SetSpellCheckerOptions(this.tbClass, optionsSpelling8);
            this.tbClass.TabIndex = 32;
            this.tbClass.Text = "Defination";
            // 
            // spellChecker1
            // 
            this.spellChecker1.Culture = new System.Globalization.CultureInfo("en-US");
            this.spellChecker1.ParentContainer = null;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 591);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(548, 13);
            this.spellChecker1.SetSpellCheckerOptions(this.textBox1, optionsSpelling9);
            this.textBox1.TabIndex = 33;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Copyright 2013 C & W Information Technology, LLC";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbMaxLength
            // 
            this.tbMaxLength.Location = new System.Drawing.Point(7, 13);
            this.tbMaxLength.Name = "tbMaxLength";
            this.tbMaxLength.Size = new System.Drawing.Size(57, 20);
            this.spellChecker1.SetSpellCheckerOptions(this.tbMaxLength, optionsSpelling10);
            this.tbMaxLength.TabIndex = 0;
            this.tbMaxLength.Text = "50";
            // 
            // gbMaxLength
            // 
            this.gbMaxLength.Controls.Add(this.tbMaxLength);
            this.gbMaxLength.Location = new System.Drawing.Point(133, 160);
            this.gbMaxLength.Name = "gbMaxLength";
            this.gbMaxLength.Size = new System.Drawing.Size(77, 36);
            this.gbMaxLength.TabIndex = 34;
            this.gbMaxLength.TabStop = false;
            this.gbMaxLength.Text = "Max Length";
            this.gbMaxLength.Visible = false;
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(548, 609);
            this.Controls.Add(this.gbMaxLength);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tbClass);
            this.Controls.Add(this.tbClassName);
            this.Controls.Add(this.btnGenerateClass);
            this.Controls.Add(this.cbPropertyType);
            this.Controls.Add(this.btnSwitch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbRelationshipName);
            this.Controls.Add(this.tbClassTwo);
            this.Controls.Add(this.btnCopy2);
            this.Controls.Add(this.btnCopy1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbDefinationMany);
            this.Controls.Add(this.tbPropertyNameOne);
            this.Controls.Add(this.tbPropertyNameTwo);
            this.Controls.Add(this.tbDefinationOne);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Property Name";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbPropertyType.Properties)).EndInit();
            this.gbMaxLength.ResumeLayout(false);
            this.gbMaxLength.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox tbDefinationOne;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TextBox tbPropertyNameTwo;
        private System.Windows.Forms.TextBox tbPropertyNameOne;
        private System.Windows.Forms.TextBox tbDefinationMany;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbProperty;
        private System.Windows.Forms.RadioButton rbOne2One;
        private System.Windows.Forms.RadioButton rbMany2Many;
        private System.Windows.Forms.Button btnCopy2;
        private System.Windows.Forms.TextBox tbClassTwo;
        private System.Windows.Forms.TextBox tbRelationshipName;
        private System.Windows.Forms.RadioButton rbOne2Many;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCopy1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ckbDropFirstLetter;
        private System.Windows.Forms.Button btnSwitch;
        private DevExpress.XtraEditors.ComboBoxEdit cbPropertyType;
        private System.Windows.Forms.Button btnGenerateClass;
        private System.Windows.Forms.TextBox tbClassName;
        private System.Windows.Forms.TextBox tbClass;
        private DevExpress.XtraSpellChecker.SpellChecker spellChecker1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox gbMaxLength;
        private System.Windows.Forms.TextBox tbMaxLength;
        private System.Windows.Forms.ToolStripMenuItem toggleOnTopToolStripMenuItem;
    }
}

