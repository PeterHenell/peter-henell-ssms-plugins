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

        public static void AppendDropTempTableIfExists(this StringBuilder sb, string tempTableName) {
            sb.AppendFormat("IF OBJECT_ID('TempDB..{0}') IS NOT NULL DROP TABLE {0};", tempTableName);
        }

        public static void AppendTempTablesFor(this StringBuilder sb, DataTable metaTable, string tempTableName)
        {
            sb.AppendTableDefinitionFrom(metaTable, tempTableName);
            sb.AppendLine();
        }

        public static void AppendTempTablesFor(this StringBuilder sb, DataSet ds)
        {
            int tableCounter = 1;
            foreach (DataTable metaTable in ds.Tables)
            {
                string tableName = string.Format("{0}{1}", "#temp", tableCounter++);
                sb.AppendDropTempTableIfExists(tableName);
                sb.AppendTableDefinitionFrom(metaTable, tableName);
            }
        }

        public static void AppendColumnNameList(this StringBuilder sb, DataTable dt, bool newLineBetweenColumns = true, string columnDelimiter = " ")
        {
            int columnCount = 1;
            foreach (DataColumn col in dt.Columns)
            {
                sb.AppendFormat("{1}[{0}]", col.ColumnName, columnCount > 1 ? columnDelimiter : string.Empty);

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
                sb.Append(rowSep + "  (");
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
                case "system.guid":
                case "system.datetime":
                case "system.datetimeoffset":
                    return string.Format("'{0}'", value);
                case "system.decimal":
                case "system.float":
                case "system.double":
                    return value.ToString().Replace(",", ".");
                case "system.byte[]":
                    return ByteArrayToString((byte[])value);
                default:
                    return value.ToString();
            }
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return string.Format("'0x{0}'", hex.ToString());
        }

    }
}
