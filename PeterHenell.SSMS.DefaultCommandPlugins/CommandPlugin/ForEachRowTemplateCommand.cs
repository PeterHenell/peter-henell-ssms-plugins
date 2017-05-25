using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Forms;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ForEachRowTemplateCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "ForEachRow_Command";

        public ForEachRowTemplateCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.DataGeneration,
                 "Run Template for each row of selected query",
                 "global::Ctrl+Alt+E")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
        {
            var selectedText = ShellManager.GetSelectedText();
            var sb = new StringBuilder();
            DialogManager.GetDialogInputFromUser("Enter Row Template", "Col1 = {0}, Col2 = '{1}'", template =>
            {
                using (var ds = new DataSet())
                {
                    QueryManager.Run(ConnectionManager.GetConnectionStringForCurrentWindow(), token, (queryManager) =>
                    {
                        queryManager.Fill(selectedText, ds);
                    });

                    var columnsRequested = template.Count(c => c == '{');

                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            if (row.ItemArray.Length < columnsRequested)
                            {
                                sb.AppendLine("-- the result set contain fewer columns than specified in the template");
                                break;
                            }
                            var data = row.ItemArray.Take(columnsRequested).ToArray();
                            sb.AppendLine(string.Format(template, data));
                        }
                    }
                }
            });
            ShellManager.AppendToEndOfSelection(sb.ToString());
        }
    }
}