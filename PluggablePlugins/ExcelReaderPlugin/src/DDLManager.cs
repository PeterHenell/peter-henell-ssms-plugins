using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderTest
{
    public class DDLManager
    {
        public void CreateTable(string tableName, DataTable meta, string connectionString)
        {
            var sb = new StringBuilder();
            //sb.AppendLine("IF OBJECT_ID('" + tableName + "') IS NOT NULL EXEC('DROP TABLE [" + tableName + "]');");
            sb.AppendLine("IF OBJECT_ID('" + tableName + "') IS NOT NULL RAISERROR('Table already exists', 16, 1);");

            sb.AppendLine("EXEC('create table [" + tableName + "] (");
            sb.AppendLine(string.Join(", ", meta.Columns.Cast<DataColumn>()
                                .Select(x => string.Format("[{0}] varchar(500)", x.ColumnName))));
            sb.AppendLine(")');");

            var sql = sb.ToString();
            Console.WriteLine(sql);

            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}
