using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Shell;
using RedGate.SIPFrameworkShared;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PeterHenell.SSMS.Plugins.ExtensionMethods;

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
                var sb = new StringBuilder();
                using (var ds = new DataSet())
                {
                    var queryManager = new DatabaseQueryManager(ConnectionManager.GetConnectionStringForCurrentWindow());
                    queryManager.ExecuteQuery(string.Format("SET ROWCOUNT 1; {0}", selectedText), ds);
                    sb.AppendTempTablesFor(ds);

                    if (ds.Tables.Count == 1)
                    {
                        sb.Append("INSERT INTO #temp1");

                        shellManager.AddTextToTopOfSelection(sb.ToString());

                        sb.Clear();
                        sb.AppendColumnNameList(ds.Tables[0]);
                        shellManager.AddTextToEndOfSelection(
                                string.Format("{0}SELECT{0}{1}{0}FROM #temp1", Environment.NewLine, sb.ToString())
                                );
                    }
                    else
                    {
                        shellManager.AddTextToEndOfSelection(sb.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public string Name { get { return COMMAND_NAME; } }
        public string Caption { get { return "Generate Temp Tables From Selected Queries"; } }
        public string Tooltip { get { return "Select a query, the result will be fitted into a generated temporary table."; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+D" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }

        public void SetSelectedDBNode(ObjectExplorerNodeDescriptorBase theSelectedNode)
        {
        }
    }
}