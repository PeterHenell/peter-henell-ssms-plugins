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
        private string connectionString;

        public DatabaseQueryManager(string connectionString)
        {
            this.connectionString = connectionString;
        }


        /// <summary>
        /// Fills a dataset by executing the supplied command in the current window connection
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        internal DataSet ExecuteQuery(string query, DataSet ds)
        {
            //var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();
            
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var ad = new SqlDataAdapter(cmd))
            {
                ad.Fill(ds);
                return ds;
            }
        }

        internal void ExecuteQuery(string sql, Action<SqlDataReader> streamReaderCallback)
        {
            //var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();
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
