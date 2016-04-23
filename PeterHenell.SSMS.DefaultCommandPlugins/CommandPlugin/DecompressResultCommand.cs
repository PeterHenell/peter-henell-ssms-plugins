using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Plugins;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class DecompressResultCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "DecompressResultCommand";

        public DecompressResultCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.DataGeneration,
                 "Uncompress gzipped strings",
                 "global::Ctrl+Alt+C")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
        {
            var selectedText = ShellManager.GetSelectedText();
            var sb = new StringBuilder();
            int errorCount = 0;

            var dataReaderCallback = new Action<SqlDataReader>((reader) =>
            {
                var base64String = reader.GetString(0);
                // correct base64 strings are of multiples of 4. This is a simple sanity check.
                if (base64String.Length % 4 == 0)
                    sb.AppendLine(GZipManager.DecompressBase64EncodedString(base64String));
                else
                    errorCount++;
            });

            var queryManager = new DatabaseQueryManager(ConnectionManager.GetConnectionStringForCurrentWindow());
            queryManager.ExecuteQuery(selectedText, dataReaderCallback);

            if (errorCount > 0)
                ShellManager.ShowMessageBox("Processed results with some errors, some of the rows contain non-64base strings. The successfull results will be displayed.");

            NotepadHelper.ShowInNotepad(sb.ToString());
        }

        //public string Name { get { return COMMAND_NAME; } }
        //public string Caption { get { return "Uncompress gzipped strings"; } }
        //public string Tooltip { get { return "Uncompress first field in result"; } }
        //public ICommandImage Icon { get { return m_CommandImage; } }
        //public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+C" }; } }
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