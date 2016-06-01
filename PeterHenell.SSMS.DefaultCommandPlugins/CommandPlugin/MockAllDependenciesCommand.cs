using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;
using PeterHenell.SSMS.Plugins.Plugins;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Text;

namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin
{
    public class MockAllDependenciesCommand : CommandPluginBase
    {
        public static readonly string COMMAND_NAME = "MockAllDeps_Command";

        public MockAllDependenciesCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.TSQLTTools,
                 "tSQLt - Mock All Dependencies for selected object",
                 "global::Ctrl+ALT+N")
        {
        }

        public override void ExecuteCommand(System.Threading.CancellationToken token)
        {
            var selectedText = ShellManager.GetSelectedText();

            var options = new PeterHenell.SSMS.Plugins.Utils.TsqltManager.MockOptionsDictionary();
            options.EachColumnInSelectOnNewRow = false;
            options.EachColumnInValuesOnNewRow = false;
            var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();

            var meta = ObjectMetadata.FromQualifiedString(selectedText);

            var service = new ObjectDependencyService();
            var dependencies = service.GetDependencies(meta, connectionString, token);

            var superMockingString = TsqltManager.MockAllDependencies(token, options, connectionString, dependencies);

            ShellManager.AppendToEndOfSelection(superMockingString);
        }
    }
}
