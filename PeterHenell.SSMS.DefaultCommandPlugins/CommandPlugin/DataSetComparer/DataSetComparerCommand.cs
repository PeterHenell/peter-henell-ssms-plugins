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

namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin.DataSetComparer
{
    public class DataSetComparerCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "CompareQueryResult_Command";

        public override void ExecuteCommand(CancellationToken token)
        {
            string selectedText = ShellManager.GetSelectedText();
            var sb = new StringBuilder();
            using (var ds = new DataSet())
            {
                QueryManager.Run(ConnectionManager.GetConnectionStringForCurrentWindow(), token, (queryManager) =>
                {
                    queryManager.Fill(selectedText, ds);
                });
                if (ds.Tables.Count != 2)
                {
                    throw new InvalidOperationException("Can only compare two result sets.");
                }

                var a = ds.Tables[0];
                var b = ds.Tables[1];

                DataSetComparer comparer = new DataSetComparer();
                var setsAreEqual = comparer.SetsAreEqual(a, b);

                var diff = a.AsEnumerable().Except(b.AsEnumerable());
                
                // calculate diff and present result
            }

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
