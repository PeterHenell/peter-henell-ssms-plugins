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

        readonly ISsmsFunctionalityProvider4 provider;
        readonly ICommandImage m_CommandImage = new CommandImageNone();
        readonly ShellManager shellManager;
        readonly ObjectExplorerNodeDescriptorBase currentNode = null;
        readonly TableMetaDataAccess tableMetaAccess;

        public GenerateDataForTableCommand(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
            this.shellManager = new ShellManager(provider);
            this.tableMetaAccess = new TableMetaDataAccess(ConnectionManager.GetConnectionStringForCurrentWindow());
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
            Action<string> okAction = new Action<string>(userInput =>
            {
                int numRows = 0;
                ParseParam(userInput, out numRows);

                var meta = GetTableMetaFromSelectedText();
                DataTable table = tableMetaAccess.GetTableSchema(meta);

                DataGenerator generator = new DataGenerator();
                generator.Fill(table, numRows);

                string output = GenerateInsertFor(table, meta.ToFullString());
                shellManager.AppendToEndOfSelection(output);
            });

            try
            {
                DialogManager.GetDialogInputFromUser("How many rows to generate? (0-1000)", "10", okAction, cancelCallback);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        

        private TableMetadata GetTableMetaFromSelectedText()
        {
            string selectedText = shellManager.GetSelectedText();
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