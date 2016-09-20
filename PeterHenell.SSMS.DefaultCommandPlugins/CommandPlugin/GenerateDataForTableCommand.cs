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
using PeterHenell.SSMS.Plugins.Utils.Generators;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class GenerateDataForTableCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "GenerateDataForTable_Command";

        ObjectMetadataAccess tableMetaAccess;

        public GenerateDataForTableCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.DataGeneration,
                 "Generate Insert X Rows for Selected Table",
                 "global::Ctrl+Alt+I")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
        {
            this.tableMetaAccess = new ObjectMetadataAccess(ConnectionManager.GetConnectionStringForCurrentWindow());
            Action<string> okAction = new Action<string>(userInput =>
            {
                int numRows = 0;
                ParseParam(userInput, out numRows);

                var meta = GetTableMetaFromSelectedText();
                DataTable table = tableMetaAccess.GetTableSchema(meta, token);

                DataGenerator generator = new DataGenerator();
                generator.Fill(table, numRows);

                string output = GenerateInsertFor(table, meta.ToFullString());
                ShellManager.AppendToEndOfSelection(output);
            });

            DialogManager.GetDialogInputFromUser("How many rows to generate? (0-1000)", "10", okAction, cancelCallback);
        }



        private ObjectMetadata GetTableMetaFromSelectedText()
        {
            string selectedText = ShellManager.GetSelectedText();
            var meta = ObjectMetadata.FromQualifiedString(selectedText);
            return meta;
        }

        private void ParseParam(string paramValue, out int numRows)
        {
            if (!int.TryParse(paramValue, out numRows))
            {
                throw new ArgumentException("Please input a valid number");
            }
            if (numRows > 1000)
            {
                numRows = 1000;
            }
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

    }
}