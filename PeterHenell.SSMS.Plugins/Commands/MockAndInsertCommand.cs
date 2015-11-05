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
using PeterHenell.SSMS.Plugins.Plugins;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class MockAndInsertCommand : ICommandPlugin
    {
        public readonly static string COMMAND_NAME = "MockAndInsert_Command";

        private ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;

        private readonly ICommandImage m_CommandImage = new CommandImageNone();

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
            var options = new MockOptionsDictionary();

            var ok = new Action<string, MockOptionsDictionary>((result, checkedOptions) =>
            {
                int numRows = 0;
                if (!int.TryParse(result, out numRows))
                {
                    MessageBox.Show("Please input a valid number");
                    return;
                }
                else
                {
                    if (numRows <= 0)
                    {
                        numRows = 0;
                    }
                    else if (numRows > 1000)
                    {
                        numRows = 1000;
                    }
                }

                try
                {
                    var selectedText = shellManager.GetSelectedText();

                    var meta = TableMetadata.FromQualifiedString(selectedText);
                    TableMetaDataAccess da = new TableMetaDataAccess(ConnectionManager.GetConnectionStringForCurrentWindow());
                    var table = da.SelectTopNFrom(meta, numRows);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(TsqltManager.GetFakeTableStatement(selectedText));
                    sb.AppendLine();
                    sb.Append(TsqltManager.GenerateInsertFor(table, meta, options.EachColumnInSelectOnNewRow, options.EachColumnInValuesOnNewRow));

                    shellManager.ReplaceSelectionWith(sb.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });

            var diagManager = new DialogManager.InputWithCheckboxesDialogManager<MockOptionsDictionary>();
            diagManager.Show("How many rows to select? (0=max)", "1", options, ok, cancelCallback);
        }

        public class MockOptionsDictionary : Dictionary<string, bool>
        {
            public MockOptionsDictionary()
            {
                EachColumnInSelectOnNewRow = false;
                EachColumnInValuesOnNewRow = false;
            }

            public bool EachColumnInSelectOnNewRow
            {
                get
                {
                    return this["Each Column in new row in INSERT?"];
                }
                set
                {
                    if (this.ContainsKey("Each Column in new row in INSERT?"))
                    {
                        this["Each Column in new row in INSERT?"] = value;
                        return;
                    }
                    this.Add("Each Column in new row in INSERT?", value);
                }
            }

            public bool EachColumnInValuesOnNewRow
            {
                get
                {
                    return this["Each Column in new row in VALUES?"];
                }
                set
                {
                    if (this.ContainsKey("Each Column in new row in VALUES?"))
                    {
                        this["Each Column in new row in VALUES?"] = value;
                        return;
                    }
                    this.Add("Each Column in new row in VALUES?", value);
                }
            }
        }


        private void cancelCallback()
        {
        }

        public string Name { get { return COMMAND_NAME; } }
        public string Caption { get { return "tSQLt - Mock And insert row to selected table"; } }
        public string Tooltip { get { return "Mocks And inserts a row to selected table"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+M" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }

        public string MenuGroup
        {
            get { return "TSQLT - Tools"; }
        }

        public void Init(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
            this.shellManager = new ShellManager(provider);
        }
    }
}