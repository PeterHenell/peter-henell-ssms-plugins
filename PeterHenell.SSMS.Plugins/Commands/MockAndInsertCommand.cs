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
            try
            {
                var selectedText = shellManager.GetSelectedQuery();

                var meta = TableMetadata.FromQualifiedString(selectedText);
                TableMetaDataAccess da = new TableMetaDataAccess(ConnectionManager.GetConnectionStringForCurrentWindow());
                var table = da.GetMetaDataForTable(meta);

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
        }

        private string GenerateInsertFor(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ### (");
            sb.AppendColumnNameList(dataTable);
            sb.AppendLine(")");
            sb.AppendLine("VALUES ");
            sb.AppendListOfRows(dataTable);
            sb.Append(";");
            return sb.ToString();
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