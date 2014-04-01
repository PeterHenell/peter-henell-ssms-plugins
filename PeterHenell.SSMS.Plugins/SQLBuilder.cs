using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace PeterHenell.SSMS.Plugins
{
    class SQLBuilder
    {
        internal static string CreateTempTablesFromQueryResult(string input)
        {
            var sb = new StringBuilder();

            try
            {
                SQLBuilder.CreateTemporaryTablesFromQueries(sb, input);
            }
            catch (Exception ex)
            {
                sb.AppendLine(" ... ");
                sb.AppendFormat("An error occured during generation: [{0}]", ex.ToString());
            }

            return sb.ToString();
        }

        private static void CreateTemporaryTablesFromQueries(StringBuilder sb, string input)
        {
            var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();

            var cmd = new SqlCommand(string.Format("SET FMTONLY ON; {0}", input), new SqlConnection(connectionString));
            var ad = new SqlDataAdapter(cmd);
            var ds = new DataSet();

            ad.Fill(ds);
            int tableCounter = 1;
            foreach (DataTable metaTable in ds.Tables)
            {
                string tempTableName = string.Format("#temp{0}", tableCounter++);
                sb.AppendFormat("IF OBJECT_ID('tempdb..{0}') IS NOT NULL DROP TABLE {0};", tempTableName);

                sb.AppendLine();
                sb.AppendFormat("CREATE TABLE {0} (", tempTableName);
                sb.AppendLine();

                int columnCount = 1;
                foreach (DataColumn col in metaTable.Columns)
                {
                    sb.AppendFormat("\t [{0}]\t{1}", col.ColumnName, TranslateToSqlType(col.DataType).ToUpper());

                    if (columnCount < metaTable.Columns.Count)
                        sb.Append(",");

                    sb.AppendLine();
                    columnCount++;
                }

                sb.Append(");");
                sb.AppendLine();
                sb.AppendLine();
            }
        }

        /// <summary>
        /// Translate .Net type to string value containing a corresponding SQL Server type.
        /// Note: Some datatypes will have precision defined that is not exactly same as it was in source tables.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string TranslateToSqlType(System.Type type)
        {
            var dbt = GetDBType(type);
            switch (dbt)
            {
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                    return dbt.ToString() + "(max)";
                case SqlDbType.DateTime:
                    return SqlDbType.DateTime2.ToString();
                case SqlDbType.Decimal:
                    return dbt.ToString() + "(19,6)";
                default:
                    return dbt.ToString();
            }

        }

        /// <summary>
        /// Stolen with pride from http://www.codeproject.com/Articles/16706/Convert-System-Type-to-SqlDbType
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        private static SqlDbType GetDBType(System.Type theType)
        {
            System.Data.SqlClient.SqlParameter p1 = null;
            System.ComponentModel.TypeConverter tc = null;
            p1 = new System.Data.SqlClient.SqlParameter();
            tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType);
            if (tc.CanConvertFrom(theType))
            {
                p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
            }
            else
            {
                //Try brute force
                try
                {
                    p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
                }
                catch (Exception)
                {
                    //Do Nothing
                }
            }
            return p1.SqlDbType;
        }
    }
}
