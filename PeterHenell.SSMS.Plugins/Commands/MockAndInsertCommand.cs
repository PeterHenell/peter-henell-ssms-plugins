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

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class MockAndInsertCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "MockAndInsert_Command";

        private readonly ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;

        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public MockAndInsertCommand(ISsmsFunctionalityProvider4 provider)
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
                } else
                {
                    if (numRows <= 0)
                    {
                        numRows = 0;
                    }
                    else if (numRows > 1000)
                    {
                        numRows = 1000;
                    }
                }

                try
                {
                    var selectedText = shellManager.GetSelectedText();

                    var meta = TableMetadata.FromQualifiedString(selectedText);
                    TableMetaDataAccess da = new TableMetaDataAccess(ConnectionManager.GetConnectionStringForCurrentWindow());
                    var table = da.SelectTopNFrom(meta, numRows);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(TsqltManager.GetFakeTableStatement(selectedText));
                    sb.AppendLine();
                    sb.Append(TsqltManager.GenerateInsertFor(table, meta));

                    shellManager.ReplaceSelectionWith(sb.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });

            DialogManager.GetDialogInputFromUser("How many rows to select? (0=max)", "1", ok, cancelCallback);
        }


        private void cancelCallback()
        {
        }

        public string Name { get { return COMMAND_NAME; } }
        public string Caption { get { return "tSQLt - Mock And insert row to selected table"; } }
        public string Tooltip { get { return "Mocks And inserts a row to selected table"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+M" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }
    }
}