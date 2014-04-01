using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using PeterHenell.SSMS.Plugins.ExtensionMethods;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    class DatabaseQueryManager
    {
        internal static string CreateTempTablesFromQueryResult(string sqlQuery)
        {
            try
            {
                var sb = new StringBuilder();
                CreateTemporaryTablesFromQueries(sb, sqlQuery);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("An error occured during generation: [{0}]", ex.Message));
            }
        }

        private static void CreateTemporaryTablesFromQueries(StringBuilder sb, string sqlQuery)
        {
            var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();
            
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(string.Format("SET ROWCOUNT 1; {0}", sqlQuery), con))
            using (var ad = new SqlDataAdapter(cmd))
            using (var ds = new DataSet())
            {
                ad.Fill(ds);
                sb.AppendTempTablesFor(ds);
            }
        }
    }
}
