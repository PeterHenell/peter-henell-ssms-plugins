using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin.DataSetComparer;
using PeterHenell.SSMS.Plugins.Logging;

namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin.DataSetComparer
{
    public class DataSetComparerCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "CompareQueryResult_Command";

        public override void ExecuteCommand(CancellationToken token)
        {
            string selectedText = ShellManager.GetSelectedText();
            var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();
            var sb = GetComparisonResultFrom(ref token, selectedText, connectionString);
            ShellManager.AppendToEndOfSelection(sb.ToString());

        }

        public StringBuilder GetComparisonResultFrom(ref CancellationToken token, string selectedText, string connectionString)
        {
            var sb = new StringBuilder();
            using (var ds = new DataSet())
            {
                QueryManager.Run(connectionString, token, (queryManager) =>
                {
                    queryManager.Fill(selectedText, ds);
                });
                if (ds.Tables.Count != 2)
                {
                    throw new InvalidOperationException("Can only compare two result sets.");
                }

                var a = ds.Tables[0];
                var b = ds.Tables[1];
                var logger = new StringBuilderTextWriter(sb);

                var comp = DataSetTools.CompareBuilder.Build("", logger, (comparer) => comparer
                             .Compare(a, "First")
                             .To(b, "Second")
                             .End());

                var result = comp.CompareCommands();
                result.FormatResult(sb);

            }
            return sb;
        }

        public DataSetComparerCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.DataGeneration,
                 "Compare the result of two queries",
                 "global::Ctrl+Alt+C")
        {
        }
    }
}
