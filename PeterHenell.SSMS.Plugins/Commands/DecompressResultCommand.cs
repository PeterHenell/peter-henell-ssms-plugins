using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using RedGate.SIPFrameworkShared;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class DecompressResultCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "DecompressResultCommand";

        private readonly ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;
        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public DecompressResultCommand(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
            this.shellManager = new ShellManager(provider);
        }

        public void Execute(object parameter)
        {
            try
            {
                PerformCommand();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PerformCommand()
        {
            var selectedText = shellManager.GetSelectedQuery();
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

            DataAccess.DatabaseQueryManager.ExecuteQuery(selectedText, dataReaderCallback);

            if (errorCount > 0)
                MessageBox.Show("Processed results with some errors, some of the rows contain non-64base strings. The successfull results will be displayed.");

            NotepadHelper.ShowInNotepad(sb.ToString());
        }

        public string Name { get { return COMMAND_NAME; } }
        public string Caption { get { return "Uncompress gzipped strings"; } }
        public string Tooltip { get { return "Uncompress first field in result"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+C" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }
    }
}