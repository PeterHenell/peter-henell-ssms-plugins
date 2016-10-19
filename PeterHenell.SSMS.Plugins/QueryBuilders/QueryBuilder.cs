using PeterHenell.SSMS.Plugins.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.QueryBuilders
{
    public static class QueryBuilder
    {
        public static void AppendTableDefinitionFrom(TextWriter tw, DataTable metaTable, string tempTableName)
        {
            tw.WriteLine();
            tw.Write("CREATE TABLE {0} (", tempTableName);
            tw.WriteLine();

            int columnCount = 1;
            foreach (DataColumn col in metaTable.Columns)
            {
                tw.Write("\t[{0}]\t{1}", col.ColumnName, DbTypeConverter.TranslateToSqlType(col.DataType).ToUpper());

                if (columnCount++ < metaTable.Columns.Count)
                    tw.Write(",");

                tw.WriteLine();
            }

            tw.Write(");");
            tw.WriteLine();
        }

        public static void AppendDropTempTableIfExists(TextWriter tw, string tempTableName)
        {
            tw.Write("IF OBJECT_ID('TempDB..{0}') IS NOT NULL DROP TABLE {0};", tempTableName);
        }

        public static void AppendTempTablesFor(TextWriter tw, DataTable metaTable, string tempTableName)
        {
            AppendTableDefinitionFrom(tw, metaTable, tempTableName);
            tw.WriteLine();
        }

        public static void AppendTempTablesFor(TextWriter tw, DataSet ds)
        {
            int tableCounter = 1;
            foreach (DataTable metaTable in ds.Tables)
            {
                string tableName = string.Format("{0}{1}", "#temp", tableCounter++);
                AppendDropTempTableIfExists(tw, tableName);
                AppendTableDefinitionFrom(tw, metaTable, tableName);
            }
        }

        public static void AppendColumnNameList(TextWriter tw, DataTable dt, bool newLineBetweenColumns = true, string columnDelimiter = " ")
        {
            int columnCount = 1;
            foreach (DataColumn col in dt.Columns)
            {
                tw.Write("{1}[{0}]", col.ColumnName, columnCount > 1 ? columnDelimiter : string.Empty);

                if (columnCount++ < dt.Columns.Count)
                    tw.Write(",");
                if (newLineBetweenColumns)
                    tw.WriteLine();
            }
        }
        public static void AppendListOfRows(TextWriter tw, DataTable dataTable, bool newLineBetweenValues = false)
        {
            string rowSep = "";
            string colSep = "";
            foreach (DataRow row in dataTable.Rows)
            {
                colSep = "";
                tw.Write(rowSep + "  (");
                foreach (DataColumn col in dataTable.Columns)
                {
                    var value = GetValue(row, col);
                    tw.Write(colSep + value);
                    colSep = ", ";
                    if (newLineBetweenValues)
                        tw.WriteLine();
                }
                tw.Write(")");
                rowSep = ", " + Environment.NewLine;
            }
        }
        public static void AppendListOfSelects(TextWriter tw, DataTable dataTable)
        {
            string rowSep = "";
            string colSep = "";
            foreach (DataRow row in dataTable.Rows)
            {
                colSep = "";
                tw.Write(rowSep + "  SELECT ");
                foreach (DataColumn col in dataTable.Columns)
                {
                    var value = GetValue(row, col).ToString().Replace("'", "''");
                    tw.Write(colSep + value);
                    colSep = ", ";
                }
                rowSep = Environment.NewLine;
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
