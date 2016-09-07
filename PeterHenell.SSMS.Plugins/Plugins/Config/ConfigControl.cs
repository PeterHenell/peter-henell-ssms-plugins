using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Plugins.Config
{
    public partial class ConfigControl : UserControl
    {
        public ConfigControl()
        {
            InitializeComponent();
        }

        private void ConfigControl_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

    }
}
