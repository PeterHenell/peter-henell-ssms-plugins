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
        public static void AppendColumnNameList(this StringBuilder sb, DataTable dt, bool newLineBetweenColumns = true)
        {
            int columnCount = 1;
            foreach (DataColumn col in dt.Columns)
            {
                sb.AppendFormat("\t[{0}]", col.ColumnName);

                if (columnCount++ < dt.Columns.Count)
                    sb.Append(",");
                if (newLineBetweenColumns)
                    sb.AppendLine();
            }
        }
        public static void AppendListOfRows(this StringBuilder sb, DataTable dataTable, bool newLineBetweenValues = false)
        {
            string rowSep = "";
            string colSep = "";
            foreach (DataRow row in dataTable.Rows)
            {
                colSep = "";
                sb.Append(rowSep + "\t(");
                foreach (DataColumn col in dataTable.Columns)
                {
                    var value = GetValue(row, col);
                    sb.Append(colSep + value);
                    colSep = ", ";
                    if (newLineBetweenValues)
                        sb.AppendLine();
                }
                sb.Append(")");
                rowSep = ", " + Environment.NewLine;
            }
        }
        private static object GetValue(DataRow row, DataColumn col)
        {
            var value = row[col];

            if (row.IsNull(col))
            {
                return "NULL";
            }

            switch (col.DataType.ToString().ToLowerInvariant())
            {
                case "system.boolean":
                    return ((bool)value) ? 1 : 0;
                case "system.string":
                    return string.Format("'{0}'", value);
                case "system.datetime":
                    return string.Format("'{0}'", value);
                case "system.decimal":
                    return value.ToString().Replace(",", ".");
                    
                default:
                    return value.ToString();
            }

        }

    }
}
