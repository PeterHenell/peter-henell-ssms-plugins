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
using PeterHenell.SSMS.Plugins.Utils.Generators;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class GenerateDataForTableCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "GenerateDataForTable_Command";

        private readonly ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;
        private ObjectExplorerNodeDescriptorBase currentNode = null;

        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public GenerateDataForTableCommand(ISsmsFunctionalityProvider4 provider)
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
                if (numRows > 1000)
                {
                    numRows = 1000;
                }

                string selectedText = shellManager.GetSelectedText();
                var meta = TableMetadata.FromQualifiedString(selectedText);
                
                DataSet ds = new DataSet();
                string query = string.Format(@"
set rowcount {0}; 
select * from {1}; 
set rowcount 0;", numRows, meta.ToFullString());
                var queryManager = new DatabaseQueryManager(ConnectionManager.GetConnectionStringForCurrentWindow());
                queryManager.ExecuteQuery(query, ds);

                if (ds.Tables.Count > 0)
                {
                    var table = ds.Tables[0];
                    DataGenerator generator = new DataGenerator();
                    generator.Fill(table, numRows);
                    
                    string output = GenerateInsertFor(table, meta.ToFullString());
                    shellManager.AddTextToEndOfSelection(output);
                }
                else
                {
                    MessageBox.Show("Query did not produce any result");
                }
            });

            DialogManager.GetDialogInputFromUser("How many rows to generate? (0-1000)", "10", ok, cancelCallback);
        }

        private string GenerateInsertFor(DataTable dataTable, string targetTable)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO " + targetTable + " (");
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
        public string Caption { get { return "Generate Insert X Rows for Selected Table"; } }
        public string Tooltip { get { return "Generate Insert With Generated Rows for Selected Table"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+I" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }
    }
}