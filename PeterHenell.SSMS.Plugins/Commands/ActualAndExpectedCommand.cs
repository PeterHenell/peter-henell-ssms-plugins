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

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ActualAndExpectedCommand : ISharedCommandWithExecuteParameter
    {
        public readonly static string COMMAND_NAME = "ActualAndExpectedCommand";

        private readonly ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;

        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public ActualAndExpectedCommand(ISsmsFunctionalityProvider4 provider)
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
                        string selectedText = shellManager.GetSelectedText();
                        var sb = new StringBuilder();
                        using (var ds = new DataSet())
                        {
                            var queryManager = new DatabaseQueryManager(ConnectionManager.GetConnectionStringForCurrentWindow());
                            queryManager.ExecuteQuery(string.Format("SET ROWCOUNT {0}; {1}", numRows, selectedText), ds);
                            
                            if (ds.Tables.Count == 1)
                            {
                                sb.AppendDropTempTableIfExists("#Actual");
                                sb.AppendLine();
                                sb.AppendDropTempTableIfExists("#Expected");
                                sb.AppendLine();

                                sb.AppendTempTablesFor(ds.Tables[0], "#Actual");
                                sb.Append("INSERT INTO #Actual");

                                shellManager.AddTextToTopOfSelection(sb.ToString());

                                sb.Clear();
                                sb.AppendColumnNameList(ds.Tables[0]);
                                shellManager.AddTextToEndOfSelection(
                                        string.Format("{0}SELECT {1}INTO #Expected{0}FROM #Actual{0}WHERE 1=0;{0}", Environment.NewLine, sb.ToString())
                                        );
                                shellManager.AddTextToEndOfSelection(
                                    TsqltManager.GenerateInsertFor(ds.Tables[0], TableMetadata.FromQualifiedString("#Expected"), false, false));
                            }
                            else
                            {
                                return;
                            }
                        }
                    
                    //var meta = TableMetadata.FromQualifiedString(selectedText);
                    //TableMetaDataAccess da = new TableMetaDataAccess(ConnectionManager.GetConnectionStringForCurrentWindow());
                    //var table = da.SelectTopNFrom(meta, numRows);

                    //StringBuilder sb = new StringBuilder();
                    //sb.Append(TsqltManager.GetFakeTableStatement(selectedText));
                    //sb.AppendLine();
                    //sb.Append(TsqltManager.GenerateInsertFor(table, meta, options.EachColumnInSelectOnNewRow, options.EachColumnInValuesOnNewRow));
                    //shellManager.ReplaceSelectionWith(sb.ToString());
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
        public string Caption { get { return "tSQLt - Create #Actual and #Expected tables from selected query"; } }
        public string Tooltip { get { return "Generate the two temporary tables #Actual and #Expected based on current query"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+L" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {

        }
    }
}