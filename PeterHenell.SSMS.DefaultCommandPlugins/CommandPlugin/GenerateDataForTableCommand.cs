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

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class GenerateDataForTableCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "GenerateDataForTable_Command";

        TableMetaDataAccess tableMetaAccess;

        public GenerateDataForTableCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.DataGeneration,
                 "Generate Insert X Rows for Selected Table",
                 "global::Ctrl+Alt+I")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
        {
            this.tableMetaAccess = new TableMetaDataAccess(ConnectionManager.GetConnectionStringForCurrentWindow());
            Action<string> okAction = new Action<string>(userInput =>
            {
                int numRows = 0;
                ParseParam(userInput, out numRows);

                var meta = GetTableMetaFromSelectedText();
                DataTable table = tableMetaAccess.GetTableSchema(meta);

                DataGenerator generator = new DataGenerator();
                generator.Fill(table, numRows);

                string output = GenerateInsertFor(table, meta.ToFullString());
                ShellManager.AppendToEndOfSelection(output);
            });


            DialogManager.GetDialogInputFromUser("How many rows to generate? (0-1000)", "10", okAction, cancelCallback);
        }



        private TableMetadata GetTableMetaFromSelectedText()
        {
            string selectedText = ShellManager.GetSelectedText();
            var meta = TableMetadata.FromQualifiedString(selectedText);
            return meta;
        }

        private void ParseParam(string result, out int numRows)
        {
            if (!int.TryParse(result, out numRows))
            {
                throw new InvalidOperationException("Please input a valid number");
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

        //public string Name { get { return COMMAND_NAME; } }
        //public string Caption { get { return "Generate Insert X Rows for Selected Table"; } }
        //public string Tooltip { get { return "Generate Insert With Generated Rows for Selected Table"; } }
        //public ICommandImage Icon { get { return m_CommandImage; } }
        //public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+I" }; } }
        //public bool Visible { get { return true; } }
        //public bool Enabled { get { return true; } }

        //public void Execute()
        //{

        //}

        //public string MenuGroup
        //{
        //    get { return "Data Generation"; }
        //}

        //public void Init(ISsmsFunctionalityProvider4 provider)
        //{
        //    this.provider = provider;
        //    this.shellManager = new ShellManager(provider);
        //}
    }
}