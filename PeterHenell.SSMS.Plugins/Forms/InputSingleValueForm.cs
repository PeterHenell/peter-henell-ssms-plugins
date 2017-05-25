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
    public partial class InputSingleValueForm : Form
    {
        public InputSingleValueForm()
        {
            InitializeComponent();
        }

        public InputSingleValueForm(string questionText, string defaultAnswer)
        {
            InitializeComponent();

            this.input_txt.Text = defaultAnswer;
            this.Text = questionText;
            this.description_lbl.Text = questionText;
        }

        private void InputSingleValueForm_Shown(object sender, EventArgs e)
        {
            this.input_txt.SelectAll();
            this.input_txt.Focus();
        }
    }
}
