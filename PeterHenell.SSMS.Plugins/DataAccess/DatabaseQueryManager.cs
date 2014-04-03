using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using System.Collections.Generic;

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
            using (var ds = new DataSet())
            {
                ExecuteQuery(string.Format("SET ROWCOUNT 1; {0}", sqlQuery), ds);
                sb.AppendTempTablesFor(ds);
            }
        }

        /// <summary>
        /// Fills a dataset by executing the supplied command in the current window connection
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        internal static DataSet ExecuteQuery(string query, DataSet ds)
        {
            var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();
            
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var ad = new SqlDataAdapter(cmd))
            {
                ad.Fill(ds);
                return ds;
            }
        }

        internal static void ExecuteQuery(string sql, Action<SqlDataReader> streamReaderCallback)
        {
            var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();
            using (var con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        streamReaderCallback(reader);
                    }
                }
            }
        }
    }
}
