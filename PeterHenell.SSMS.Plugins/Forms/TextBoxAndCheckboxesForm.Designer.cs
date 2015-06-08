namespace PeterHenell.SSMS.Plugins.Forms
{
    partial class TextBoxAndCheckboxesForm<T> where T : System.Collections.Generic.Dictionary<string, bool>
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
            this.input_txt = new System.Windows.Forms.TextBox();
            this.cancel_button = new System.Windows.Forms.Button();
            this.ok_button = new System.Windows.Forms.Button();
            this.description_lbl = new System.Windows.Forms.Label();
            this.optionsCheckboxList = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // input_txt
            // 
            this.input_txt.Location = new System.Drawing.Point(12, 26);
            this.input_txt.Name = "input_txt";
            this.input_txt.Size = new System.Drawing.Size(100, 20);
            this.input_txt.TabIndex = 6;
            // 
            // cancel_button
            // 
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Location = new System.Drawing.Point(200, 23);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(118, 23);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 4;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            // 
            // description_lbl
            // 
            this.description_lbl.AutoSize = true;
            this.description_lbl.Location = new System.Drawing.Point(13, 7);
            this.description_lbl.Name = "description_lbl";
            this.description_lbl.Size = new System.Drawing.Size(35, 13);
            this.description_lbl.TabIndex = 7;
            this.description_lbl.Text = "label1";
            // 
            // optionsCheckboxList
            // 
            this.optionsCheckboxList.FormattingEnabled = true;
            this.optionsCheckboxList.Location = new System.Drawing.Point(12, 55);
            this.optionsCheckboxList.Name = "optionsCheckboxList";
            this.optionsCheckboxList.Size = new System.Drawing.Size(263, 139);
            this.optionsCheckboxList.TabIndex = 8;
            // 
            // TextBoxAndCheckboxesForm
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_button;
            this.ClientSize = new System.Drawing.Size(281, 206);
            this.Controls.Add(this.optionsCheckboxList);
            this.Controls.Add(this.description_lbl);
            this.Controls.Add(this.input_txt);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Name = "TextBoxAndCheckboxesForm";
            this.Text = "TextBoxAndCheckboxesForm";
            this.Shown += new System.EventHandler(this.TextBoxAndCheckboxesForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox input_txt;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Label description_lbl;
        public System.Windows.Forms.CheckedListBox optionsCheckboxList;
    }
}