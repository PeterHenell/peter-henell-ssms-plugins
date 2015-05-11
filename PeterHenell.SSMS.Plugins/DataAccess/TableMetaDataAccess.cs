using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    class TableMetaDataAccess
    {
        private string connectionString;
        public TableMetaDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataTable SelectTopNFrom(TableMetadata table, int limit = 1)
        {
            DatabaseQueryManager queryManager = new DatabaseQueryManager(connectionString);
            string query = String.Format(@"set rowcount {1}; select * from {0}; set rowcount 0;", table.ToFullString(), limit);

            DataSet ds = new DataSet();
            queryManager.ExecuteQuery(query, ds);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                throw new ArgumentException("table does not exist");
            }
        }
        
       
    }
}
