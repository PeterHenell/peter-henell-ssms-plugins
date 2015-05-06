using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Forms;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using RedGate.SIPFrameworkShared;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using System.IO;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ResultToExcelCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "ResultToExcel_Command";

        private readonly ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;

        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public ResultToExcelCommand(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
            this.shellManager = new ShellManager(provider);
        }

        public void Execute(object parameter)
        {
            try
            {
                PerformCommand();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void PerformCommand()
        {
            Action<string> ok = new Action<string>(result =>
            {
                int numRows = 0;
                if (!int.TryParse(result, out numRows))
                {
                    MessageBox.Show("Please input a valid number");
                    return;
                }

                numRows = Math.Max(numRows, 0);

                try
                {
                    var selectedQuery = shellManager.GetSelectedText();
                    DataAccess.DatabaseQueryManager query = new DatabaseQueryManager(ConnectionManager.GetConnectionStringForCurrentWindow());
                    var ds = new DataSet();

                    FileInfo file = DialogManager.ShowExcelSaveFileDialog();
                    if (file == null)
                        return;

                    query.ExecuteQuery(selectedQuery, ds);
                    ExcelManager.TableToExcel(ds, file);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });

            DialogManager.GetDialogInputFromUser("How many rows to select? (0=max)", "0", ok, cancelCallback);
        }

        private void cancelCallback()
        {
        }

        public string Name { get { return COMMAND_NAME; } }
        public string Caption { get { return "Execute and save result as Excel"; } }
        public string Tooltip { get { return "Executes and saves the result of the selected command as an Excel file"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+E" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }
    }
}