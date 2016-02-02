using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    public class DDLManager
    {
        public void CreateTable(string tableName, ExcelSheetMetadata meta)
        {
            var sb = new StringBuilder();
            sb.AppendLine("create table [" + tableName + "] (");
            sb.AppendLine(string.Join(", ", meta.Columns.Select(x => string.Format("[{0}] varchar(max)", x.ColumnName))));
            sb.AppendLine(");");

            using(var con = new SqlConnection(ConnectionManager.GetConnectionStringForCurrentWindow()))
            using(var cmd = new SqlCommand(sb.ToString(), con))
            {
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}
