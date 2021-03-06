﻿using PeterHenell.SSMS.Plugins.DataAccess;
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
using System.IO;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Threading;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ResultToExcelCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "ResultToExcel_Command";

        public override void ExecuteCommand(CancellationToken token)
        {
            Action<string> ok = new Action<string>(result =>
            {
                int numRows = 0;
                if (!int.TryParse(result, out numRows))
                {
                    ShellManager.ShowMessageBox("Please input a valid number");
                    return;
                }

                numRows = Math.Max(numRows, 0);

                var selectedQuery = ShellManager.GetSelectedText();
                QueryManager.Run(ConnectionManager.GetConnectionStringForCurrentWindow(), token, (queryManager) =>
                {
                    var ds = new DataSet();

                    FileInfo file = DialogManager.ShowExcelSaveFileDialog();
                    if (file == null)
                        return;

                    queryManager.Fill(selectedQuery, ds);
                    ExcelManager.TableToExcel(ds, file);
                });

            });

            DialogManager.GetDialogInputFromUser("How many rows to select? (0=max)", "0", ok, cancelCallback);
        }

        private void cancelCallback()
        {
        }

        public ResultToExcelCommand() :
            base(COMMAND_NAME,
                  CommandPluginBase.MenuGroups.DataGeneration,
                  "Execute and save result as Excel",
                  "global::Ctrl+Alt+E")
        {

        }
    }
}