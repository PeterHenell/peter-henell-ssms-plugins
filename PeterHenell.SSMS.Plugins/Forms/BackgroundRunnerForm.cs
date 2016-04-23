using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Forms
{
    public partial class BackgroundRunnerForm : Form
    {
        private Action runningAction;

        public BackgroundRunnerForm()
        {
            InitializeComponent();
        }

        public BackgroundRunnerForm(string header, string labelMessage, Action runningAction)
        {
            InitializeComponent();

            this.Text = header;
            this.statusLbl.Text = labelMessage;
            this.runningAction = runningAction;
            runningAction.BeginInvoke(callback, null);
        }

        private void callback(IAsyncResult ar)
        {
            this.Close();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            runningAction.EndInvoke(null);
            this.Close();
        }
    }
}
