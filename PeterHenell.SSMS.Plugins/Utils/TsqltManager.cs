using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class TsqltManager
    {
        public static string GetFakeTableStatement(string selectedText)
        {
            var table = TableMetadata.FromQualifiedString(selectedText);

            return FakeTable(table);
        }

        private static string FakeTable(TableMetadata table)
        {
            return string.Format("EXEC {0}tSQLt.FakeTable '{1}{2}';",
                table.DatabaseName != null ? table.WrappedDatabaseName + "." : "",
                table.SchemaName != null ? table.SchemaName + "." : "",
                table.TableName);
        }

        public static string GenerateInsertFor(DataTable table, TableMetadata meta, bool newLineBetweenColumns = true, bool newLineBetweenValues = false)
        {
            if (table.Rows.Count == 0)
            {
                AddRowWithDefaultValuesTo(table);
                
            }
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO " + meta.ToFullString() + " (");
            sb.AppendColumnNameList(table, newLineBetweenColumns);
            sb.AppendLine(")");
            sb.Append("VALUES");
            sb.AppendListOfRows(table, newLineBetweenValues);
            sb.Append(";");
            return sb.ToString();
        }

        private static void AddRowWithDefaultValuesTo(DataTable table)
        {
            var row = table.NewRow();
            foreach (DataColumn col in table.Columns)
            {
                object value = null;
                switch (col.DataType.ToString().ToLowerInvariant())
                {
                    case "system.boolean":
                        value=  true;
                        break;
                    case "system.string":
                        value = "abc";
                        break;
                    case "system.datetime":
                        value = DateTime.Now.ToShortDateString();
                        break;
                    case "system.decimal":
                        value = 1.0M;
                        break;
                    default:
                        value = 123;
                        break;
                }
                row[col] = value;
            }
            table.Rows.Add(row);
        }
    }
}
