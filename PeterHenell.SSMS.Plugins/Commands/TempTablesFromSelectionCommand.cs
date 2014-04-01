using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Shell;
using RedGate.SIPFrameworkShared;
using System;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class TempTablesFromSelectionCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "GenerateTempTablesFromSelectedQuery_Command";
        
        private readonly ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;
        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public TempTablesFromSelectionCommand(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
            this.shellManager = new ShellManager(provider);
        }

        
        public void Execute(object parameter)
        {
            PerformCommand();
        }

        private void PerformCommand()
        {
            try
            {
                string selectedText = shellManager.GetSelectedText();
                string tempTableDefinitions = DatabaseQueryManager.CreateTempTablesFromQueryResult(selectedText);
                shellManager.AddTextToEndOfSelection(tempTableDefinitions);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public string Name { get { return COMMAND_NAME ; } }
        public string Caption { get { return "Generate Temp Tables From Selected Queries"; } }
        public string Tooltip { get { return "Select a query, the result will be fitted into a generated temporary table."; }}
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+D" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {
            
        }
    }
}