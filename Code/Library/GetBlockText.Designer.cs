namespace ExpressHelper1011.Library
{
    partial class GetBlockText
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
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling2 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            this.tbText = new DevExpress.XtraEditors.MemoEdit();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblLine = new System.Windows.Forms.Label();
            this.spellChecker1 = new DevExpress.XtraSpellChecker.SpellChecker();
            ((System.ComponentModel.ISupportInitialize)(this.tbText.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(11, 25);
            this.tbText.Name = "tbText";
            this.spellChecker1.SetShowSpellCheckMenu(this.tbText, true);
            this.tbText.Size = new System.Drawing.Size(260, 96);
            this.spellChecker1.SetSpellCheckerOptions(this.tbText, optionsSpelling2);
            this.tbText.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(115, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(196, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(9, 9);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(32, 13);
            this.lblLine.TabIndex = 4;
            this.lblLine.Text = "Enter";
            // 
            // spellChecker1
            // 
            this.spellChecker1.Culture = new System.Globalization.CultureInfo("en-US");
            this.spellChecker1.ParentContainer = null;
            // 
            // GetBlockText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 160);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.tbText);
            this.Name = "GetBlockText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get Comment";
            this.Load += new System.EventHandler(this.GetBlockText_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbText.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit tbText;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblLine;
        private DevExpress.XtraSpellChecker.SpellChecker spellChecker1;

    }
}