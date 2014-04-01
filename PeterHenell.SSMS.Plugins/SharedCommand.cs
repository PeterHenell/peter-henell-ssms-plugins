using RedGate.SIPFrameworkShared;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System;
using EnvDTE80;
using EnvDTE;
using Microsoft.SqlServer.Management.UI.VSIntegration;
//using Microsoft.SqlServer.Management.UI.ConnectionDlg;
using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using PeterHenell.SSMS.Plugins;

namespace PeterHenell.SSMS.Plugins
{
    public class SharedCommand : ISharedCommandWithExecuteParameter
    {
        private readonly ISsmsFunctionalityProvider4 m_Provider;
        private readonly ICommandImage m_CommandImage = new CommandImageNone();

        public SharedCommand(ISsmsFunctionalityProvider4 provider)
        {
            m_Provider = provider;           
        }

        public string Name { get { return "GenerateTempTablesFromSelectedQuery_Command"; } }
        public void Execute(object parameter)
        {
            PerformCommand();
        }

        private void PerformCommand()
        {
            var editPoint = GetEditPointAtBottomOfSelection();
            var currentWindow = m_Provider.GetQueryWindowManager();
            var contents = currentWindow.GetActiveAugmentedQueryWindowContents();

            if (editPoint == null || string.IsNullOrEmpty(contents) || string.IsNullOrWhiteSpace(contents))
            {
                MessageBox.Show("Select the query you wish to generate temporary tables for and then run the command again.");
                return;
            }

            string tempTableDefinitions = CreateTempTablesFromQueryResult(contents);
            editPoint.Insert("\n" + tempTableDefinitions);
        }

        private EditPoint GetEditPointAtBottomOfSelection()
        {
            DTE2 a = (DTE2)m_Provider.SsmsDte2;
            Document document = a.ActiveDocument;
            if (document != null)
            {
                // find the selected text, and return the edit point at the bottom of it.
                TextSelection selection = document.Selection as TextSelection;
                if (selection == null)
                    return null;
                
                return selection.BottomPoint.CreateEditPoint();
            }
            return null;
        }

        

        private string CreateTempTablesFromQueryResult(string input)
        {
            var sb = new StringBuilder();

            try
            {
                SQLBuilder.CreateTemporaryTablesFromQueries(sb, input);
            }
            catch (Exception ex)
            {
                sb.AppendLine(" ... ");
                sb.AppendFormat("An error occured during generation: [{0}]", ex.ToString());
            }

            return sb.ToString();
        }

       

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