using PeterHenell.SSMS.Plugins.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.ExtensionMethods
{
    public static class TableBuilderExtensions
    {
        public static void AppendTableDefinitionFrom(this StringBuilder sb, DataTable metaTable, string tempTableName)
        {
            sb.AppendLine();
            sb.AppendFormat("CREATE TABLE {0} (", tempTableName);
            sb.AppendLine();

            int columnCount = 1;
            foreach (DataColumn col in metaTable.Columns)
            {
                sb.AppendFormat("\t[{0}]\t{1}", col.ColumnName, DbTypeConverter.TranslateToSqlType(col.DataType).ToUpper());

                if (columnCount++ < metaTable.Columns.Count)
                    sb.Append(",");

                sb.AppendLine();
            }

            sb.Append(");");
            sb.AppendLine();
        }

        public static void AppendTempTablesFor(this StringBuilder sb, DataSet ds)
        {
            int tableCounter = 1;
            foreach (DataTable metaTable in ds.Tables)
            {
                string tempTableName = string.Format("#temp{0}", tableCounter++);
                sb.AppendFormat("IF OBJECT_ID('tempdb..{0}') IS NOT NULL DROP TABLE {0};", tempTableName);

                sb.AppendTableDefinitionFrom(metaTable, tempTableName);

                sb.AppendLine();
            }
        }

    }
}
