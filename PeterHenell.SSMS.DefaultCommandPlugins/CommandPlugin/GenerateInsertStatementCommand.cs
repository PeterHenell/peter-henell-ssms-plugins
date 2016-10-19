using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Data;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Threading;
using System.IO;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class GenerateInsertStatementCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "GenerateInsertStatement_Command";

        public GenerateInsertStatementCommand() :
            base(COMMAND_NAME,
                CommandPluginBase.MenuGroups.DataGeneration,
                "Generate Insert statement for selected query",
                "global::Ctrl+Alt+G")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
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
                QueryManager.Run(ConnectionManager.GetConnectionStringForCurrentWindow(), token, (queryManager) =>
                {
                    queryManager.Fill(query, ds);
                });
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 50000)
                    {
                        //ShellManager.ShowMessageBox("The result of this query is too large to render in SSMS. Please select a location for the script to be saved.");
                        //var file = DialogManager.ShowSaveFileDialog("SQL files (*.sql)|*.sql|All files (*.*)|*.*");
                        var filename = Path.GetTempFileName();
                        using (var writer = File.CreateText(filename))
                        {
                            WriteInsertFor(ds.Tables[0], writer);
                        }
                        ShellManager.OpenFile(filename, true);
                    }
                    else
                    {
                        string output = GenerateInsertFor(ds.Tables[0]);
                        ShellManager.AppendToEndOfSelection(output);
                    }
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
            sb.AppendLine("EXEC(' ");
            sb.AppendListOfSelects(dataTable);
            sb.Append("');");
            return sb.ToString();
        }
        private void WriteInsertFor(DataTable dataTable, TextWriter writer)
        {
            writer.WriteLine("INSERT INTO ### (");
            QueryBuilders.QueryBuilder.AppendColumnNameList(writer, dataTable);
            writer.WriteLine(")");
            writer.WriteLine("EXEC(' ");
            QueryBuilders.QueryBuilder.AppendListOfSelects(writer, dataTable);
            writer.WriteLine("');");
        }

        private void cancelCallback()
        {
        }

       
    }
}