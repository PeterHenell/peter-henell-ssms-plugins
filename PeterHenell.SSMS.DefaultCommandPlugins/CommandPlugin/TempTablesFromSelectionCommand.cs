using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Shell;
using System;
using System.Data;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Threading;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class TempTablesFromSelectionCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "GenerateTempTablesFromSelectedQuery_Command";

        public override void ExecuteCommand(CancellationToken token)
        {
            string selectedText = ShellManager.GetSelectedText();
            var sb = new StringBuilder();
            using (var ds = new DataSet())
            {
                var queryManager = new DatabaseQueryManager(ConnectionManager.GetConnectionStringForCurrentWindow(), token);
                queryManager.Fill(string.Format("SET ROWCOUNT 1; {0}", selectedText), ds);
                sb.AppendTempTablesFor(ds);

                if (ds.Tables.Count == 1)
                {
                    sb.Append("INSERT INTO #Temp1");

                    ShellManager.AddTextToTopOfSelection(sb.ToString());

                    sb.Clear();
                    sb.AppendColumnNameList(ds.Tables[0]);
                    ShellManager.AppendToEndOfSelection(
                            string.Format("{0}SELECT{0}{1}{0}FROM #Temp1", Environment.NewLine, sb.ToString())
                            );
                }
                else
                {
                    ShellManager.AppendToEndOfSelection(sb.ToString());
                }
            }

        }
        public TempTablesFromSelectionCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.DataGeneration,
                 "Generate Temp Tables From Selected Queries",
                 "global::Ctrl+Alt+D")
        {

        }


        //public string Name { get { return COMMAND_NAME; } }
        //public string Caption { get { return "Generate Temp Tables From Selected Queries"; } }
        //public string Tooltip { get { return "Select a query, the result will be fitted into a generated temporary table."; } }
        //public ICommandImage Icon { get { return m_CommandImage; } }
        //public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+D" }; } }
        //public bool Visible { get { return true; } }
        //public bool Enabled { get { return true; } }

        //public void Execute()
        //{

        //}

        //public void SetSelectedDBNode(ObjectExplorerNodeDescriptorBase theSelectedNode)
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