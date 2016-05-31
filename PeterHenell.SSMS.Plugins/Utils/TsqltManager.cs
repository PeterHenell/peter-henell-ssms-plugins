using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class TsqltManager
    {
        public static string MockTableWithRows(CancellationToken token, MockOptionsDictionary options, int numRows, TableMetadata tableMeta, string connectionString)
        {
            StringBuilder sb = new StringBuilder();
            TableMetaDataAccess da = new TableMetaDataAccess(connectionString);
            var table = da.SelectTopNFrom(tableMeta, token, numRows);

            sb.Append(TsqltManager.FakeTable(tableMeta));
            sb.AppendLine();
            sb.Append(TsqltManager.GenerateInsertFor(table, tableMeta, options.EachColumnInSelectOnNewRow, options.EachColumnInValuesOnNewRow));

            return sb.ToString();
        }

        public class MockOptionsDictionary : Dictionary<string, bool>
        {
            public MockOptionsDictionary()
            {
                EachColumnInSelectOnNewRow = false;
                EachColumnInValuesOnNewRow = false;
            }

            public bool EachColumnInSelectOnNewRow
            {
                get
                {
                    return this["Each Column in new row in INSERT?"];
                }
                set
                {
                    if (this.ContainsKey("Each Column in new row in INSERT?"))
                    {
                        this["Each Column in new row in INSERT?"] = value;
                        return;
                    }
                    this.Add("Each Column in new row in INSERT?", value);
                }
            }

            public bool EachColumnInValuesOnNewRow
            {
                get
                {
                    return this["Each Column in new row in VALUES?"];
                }
                set
                {
                    if (this.ContainsKey("Each Column in new row in VALUES?"))
                    {
                        this["Each Column in new row in VALUES?"] = value;
                        return;
                    }
                    this.Add("Each Column in new row in VALUES?", value);
                }
            }
        }


        public static string GetFakeTableStatement(string selectedText)
        {
            if (string.IsNullOrEmpty(selectedText))
            {
                throw new ArgumentException("Selected text is empty");
            }
            var table = TableMetadata.FromQualifiedString(selectedText);

            return FakeTable(table);
        }

        public static string FakeTable(TableMetadata table)
        {
            return string.Format("EXEC {0}tSQLt.FakeTable '{1}{2}';",
                table.DatabaseName != null ? table.WrappedDatabaseName + "." : "",
                table.SchemaName != null ? table.SchemaName + "." : "",
                table.TableName);
        }

        public static string GenerateInsertFor(DataTable table, TableMetadata meta, bool newLineBetweenColumns = true, bool newLineBetweenValues = false)
        {
            if (table.Rows.Count == 0)
                AddRowWithDefaultValuesTo(table);
            var sb = new StringBuilder();
            sb.Append("INSERT INTO " + meta.ToFullString() + " (");
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
                        value = true;
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
