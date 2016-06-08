using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Data;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;
using System.IO;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class OpenSelectedObjectScriptCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "OpenObjectScript_Command";

        public OpenSelectedObjectScriptCommand() :
            base(COMMAND_NAME,
                CommandPluginBase.MenuGroups.Liquibase,
                "Open sql script for selected object",
                "global::Ctrl+Alt+f")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
        {
            var selectedText = ShellManager.GetSelectedText();
            var meta = ObjectMetadata.FromQualifiedString(selectedText);

            var fileName = string.Format("{0}.{1}.sql", meta.SchemaName, meta.ObjectName);

            var files = Directory.GetFiles(@"c:\src\dwh\", fileName, SearchOption.AllDirectories);

            if (files.Length == 1)
            {
                ShellManager.OpenFile(fileName: files[0], newWindow: true);
            }
            else if (files.Length == 0)
            {
                throw new FileNotFoundException("The script for: " + selectedText + " could not be found");
            }
            else
            {
                ShellManager.ShowMessageBox("Many files found");
            }
        }
    }
}