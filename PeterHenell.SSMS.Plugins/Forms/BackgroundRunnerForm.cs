using PeterHenell.SSMS.Plugins.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Forms
{
    public partial class BackgroundRunnerForm : Form
    {
        private Action startAction;
        private Action stopAction;

        public BackgroundRunnerForm()
        {
            InitializeComponent();
        }

        public BackgroundRunnerForm(string header, string labelMessage, Action startAction, Action stopAction)
        {
            InitializeComponent();

            this.Text = header;
            this.statusLbl.Text = labelMessage;
            this.startAction = startAction;
            this.stopAction = stopAction;

            startAction.BeginInvoke(callback, null);
        }

        private void callback(IAsyncResult ar)
        {
            this.Close();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            try
            {
                stopAction();
            }
            catch (Exception ex)
            {
                ShellManager.ShowMessageBox(ex.ToString());
            }
            finally
            {
                this.Close();
            }
        }
    }
}
