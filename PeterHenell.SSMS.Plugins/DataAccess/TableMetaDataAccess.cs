using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    public class TableMetaDataAccess
    {
        private string connectionString;
        public TableMetaDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataTable SelectTopNFrom(TableMetadata table, CancellationToken token, int limit = 1)
        {
            DataTable dt = QueryManager.Run<DataTable>(connectionString, token, (qm) =>
            {
                string query = String.Format(@"set rowcount {1}; select * from {0}; set rowcount 0;", table.ToFullString(), limit);

                DataSet ds = new DataSet();
                qm.Fill(query, ds);

                if (ds.Tables.Count > 0)
                {
                   return ds.Tables[0];
                }
                return null;
            });
            
            if (dt == null)
            {
                throw new ArgumentException("table does not exist");    
            }
            return dt;
        }

        public DataTable GetTableSchema(TableMetadata meta, CancellationToken token)
        {
            DataSet ds = new DataSet();
            string query = string.Format(@"
SET FMTONLY ON; 
select * from {0}; 
SET FMTONLY OFF;", meta.ToFullString());
            QueryManager.Run(connectionString, token, (qm) =>
            {
                qm.Fill(query, ds);

                if (ds.Tables.Count == 0)
                {
                    throw new InvalidOperationException("Trying to get table schema, got no result.");
                }
            });
            

            var table = ds.Tables[0];
            return table;
        }
       
    }
}
