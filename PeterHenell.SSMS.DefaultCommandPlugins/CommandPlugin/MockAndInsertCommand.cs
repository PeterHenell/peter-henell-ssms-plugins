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
    public class MockAndInsertCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "MockAndInsert_Command";

        public MockAndInsertCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.TSQLTTools,
                 "tSQLt - Mock And insert row to selected table",
                 "global::Ctrl+Alt+M")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
        {
            var options = new PeterHenell.SSMS.Plugins.Utils.TsqltManager.MockOptionsDictionary();

            var ok = new Action<string, PeterHenell.SSMS.Plugins.Utils.TsqltManager.MockOptionsDictionary>((result, checkedOptions) =>
            {
                int numRows = 0;
                if (!int.TryParse(result, out numRows))
                {
                    ShellManager.ShowMessageBox("Please input a valid number");
                    return;
                }
                else
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


                var selectedText = ShellManager.GetSelectedText();
                StringBuilder sb = new StringBuilder();
                var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();
                var meta = ObjectMetadata.FromQualifiedString(selectedText);
                sb.AppendLine(TsqltManager.MockTableWithRows(token, options, numRows, meta, connectionString));

                ShellManager.ReplaceSelectionWith(sb.ToString());

            });

            var diagManager = new DialogManager.InputWithCheckboxesDialogManager<PeterHenell.SSMS.Plugins.Utils.TsqltManager.MockOptionsDictionary>();
            diagManager.Show("How many rows to select? (0=max)", "1", options, ok, cancelCallback);
        }

        private void cancelCallback()
        {
        }
    }
}