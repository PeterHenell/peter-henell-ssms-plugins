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

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class GenerateInsertStatementCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "GenerateInsertStatement_Command";

        private readonly ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;
        private ObjectExplorerNodeDescriptorBase currentNode = null;
        
        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public GenerateInsertStatementCommand(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
            this.shellManager = new ShellManager(provider);
        }

        //public void SetSelectedDBNode(ObjectExplorerNodeDescriptorBase theSelectedNode)
        //{
        //    currentNode = theSelectedNode;

        //    var objectExplorerNode = currentNode as IOeNode;
        //    IConnectionInfo ci = null;
        //    if (objectExplorerNode != null
        //            && objectExplorerNode.HasConnection
        //            && objectExplorerNode.TryGetConnection(out ci))
        //    {
        //            this.connectionString = new SqlConnectionStringBuilder(ci.ConnectionString).ConnectionString;
        //    }
        //}

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
                long numRows = 0;
                if (!long.TryParse(result, out numRows))
	            {
                    MessageBox.Show("Please input a valid number");
                    return;
                }

                string selectedText = shellManager.GetSelectedQuery();
                DataSet ds = new DataSet();
                string query = string.Format(@"
set rowcount {0}; 
{1}; 
set rowcount 0;", numRows, selectedText);
                DataAccess.DatabaseQueryManager.ExecuteQuery(query, ds);

                if (ds.Tables.Count > 0)
                {
                    string output = GenerateInsertFor(ds.Tables[0]);
                    shellManager.AddTextToEndOfSelection(output);
                }
                else
                {
                    MessageBox.Show("Query did not produce any result");
                }
            });

            DialogManager.GetDialogInputFromUser("How many rows to select? (0=all)", "0", ok, cancelCallback);
        }

        private string GenerateInsertFor(DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ### (");
            string colSep = "";
            foreach (DataColumn col in dataTable.Columns)
            {
                sb.Append(colSep+col.ColumnName);
                colSep = ", ";
            }
            sb.AppendLine(")");
            sb.AppendLine("VALUES ");
            string rowSep = "";
            foreach (DataRow row in dataTable.Rows)
            {
                colSep = "";
                sb.Append(rowSep + "(");
                foreach (DataColumn col in dataTable.Columns)
                {
                    var value = GetValue(row, col);
                    sb.Append(colSep + value);
                    colSep = ", ";
                }
                sb.Append(")");
                rowSep = ", " + Environment.NewLine;
            }

            return sb.ToString();
        }

        private object GetValue(DataRow row, DataColumn col)
        {
            //var type = DbTypeConverter.TranslateToSqlType(col.DataType);
            var value = row[col];

            switch (col.DataType.ToString().ToLowerInvariant())
            {

                case "system.boolean":
                    return ((bool)value) ? 1 : 0;
                case "system.string":
                    return string.Format("'{0}'", value);

                default:
                    return value.ToString();
            }
            
        }

        private void cancelCallback()
        {
        }


        public string Name { get { return COMMAND_NAME; } }
        public string Caption { get { return "Generate Insert statement for selected query"; } }
        public string Tooltip { get { return "Executes the select query and produces insert statements based on the result."; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+G" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }
    }
}