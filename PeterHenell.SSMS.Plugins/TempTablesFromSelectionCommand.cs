using RedGate.SIPFrameworkShared;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins
{
    public class TempTablesFromSelectionCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "GenerateTempTablesFromSelectedQuery_Command";
        
        private readonly ISsmsFunctionalityProvider4 provider;
        private readonly ICommandImage m_CommandImage = new CommandImageNone();
        

        public TempTablesFromSelectionCommand(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;           
        }

        
        public void Execute(object parameter)
        {
            PerformCommand();
        }

        private void PerformCommand()
        {
            var editPoint = ShellManager.GetEditPointAtBottomOfSelection(provider);
            var currentWindow = provider.GetQueryWindowManager();
            var contents = currentWindow.GetActiveAugmentedQueryWindowContents();

            if (editPoint == null || string.IsNullOrEmpty(contents) || string.IsNullOrWhiteSpace(contents))
            {
                MessageBox.Show("Select the query you wish to generate temporary tables for and then run the command again.");
                return;
            }

            string tempTableDefinitions = SQLBuilder.CreateTempTablesFromQueryResult(contents);
            editPoint.Insert("\n" + tempTableDefinitions);
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