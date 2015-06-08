using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Forms
{
    public partial class TextBoxAndCheckboxesForm<TDict> : Form where TDict : Dictionary<string, bool>
    {
        private TDict _originalOptions;

        public TDict CheckedOptions
        {
            get
            {
                foreach (var key in _originalOptions.Keys.ToList())
                {
                    _originalOptions[key] = false;
                }
                
                for (int i = 0; i < optionsCheckboxList.CheckedItems.Count; i++)
                {
                    var key = (string)optionsCheckboxList.CheckedItems[i];
                    _originalOptions[key] = true;
                }
                return _originalOptions;
            }
        }

        public TextBoxAndCheckboxesForm()
        {
            InitializeComponent();
        }

        public TextBoxAndCheckboxesForm(string questionText, string defaultAnswer, TDict checkBoxOptions)
        {
            InitializeComponent();
            optionsCheckboxList.Items.Clear();
            _originalOptions = checkBoxOptions;

            foreach (var cb in checkBoxOptions)
            {
                optionsCheckboxList.Items.Add(cb.Key, cb.Value);
            }

            this.input_txt.Text = defaultAnswer;
            this.Text = questionText;
            this.description_lbl.Text = questionText;
        }

        private void TextBoxAndCheckboxesForm_Shown(object sender, EventArgs e)
        {
            this.input_txt.SelectAll();
            this.input_txt.Focus();
        }
    }
}
