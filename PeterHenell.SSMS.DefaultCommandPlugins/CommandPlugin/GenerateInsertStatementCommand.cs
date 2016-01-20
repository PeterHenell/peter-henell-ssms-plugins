using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Data;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using PeterHenell.SSMS.Plugins.Plugins;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class GenerateInsertStatementCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "GenerateInsertStatement_Command";

        public override void ExecuteCommand()
        {
            Action<string> ok = new Action<string>(result =>
            {
                long numRows = 0;
                if (!long.TryParse(result, out numRows))
                {
                    ShellManager.ShowMessageBox("Please input a valid number");
                    return;
                }

                string selectedText = ShellManager.GetSelectedText();
                DataSet ds = new DataSet();
                string query = string.Format(@"
set rowcount {0}; 
{1}; 
set rowcount 0;", numRows, selectedText);
                var queryManager = new DatabaseQueryManager(ConnectionManager.GetConnectionStringForCurrentWindow());
                queryManager.ExecuteQuery(query, ds);

                if (ds.Tables.Count > 0)
                {
                    string output = GenerateInsertFor(ds.Tables[0]);
                    ShellManager.AppendToEndOfSelection(output);
                }
                else
                {
                    ShellManager.ShowMessageBox("Query did not produce any result");
                }
            });

            DialogManager.GetDialogInputFromUser("How many rows to select? (0=max)", "0", ok, cancelCallback);
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

        public GenerateInsertStatementCommand() :
            base(COMMAND_NAME, 
                 CommandPluginBase.MenuGroups.DataGeneration, 
                 "Generate Insert statement for selected query", 
                 "global::Ctrl+Alt+G")
        {

        }
    }
}