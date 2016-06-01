using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class TsqltManager
    {
        public static string MockTableWithRows(CancellationToken token, MockOptionsDictionary options, int numRows, ObjectMetadata tableMeta, string connectionString)
        {
            StringBuilder sb = new StringBuilder();
            ObjectMetadataAccess da = new ObjectMetadataAccess(connectionString);
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
            var table = ObjectMetadata.FromQualifiedString(selectedText);

            return FakeTable(table);
        }

        public static string FakeTable(ObjectMetadata table)
        {
            return string.Format("EXEC {0}tSQLt.FakeTable '{1}{2}';",
                table.DatabaseName != null ? table.WrappedDatabaseName + "." : "",
                table.SchemaName != null ? table.SchemaName + "." : "",
                table.ObjectName);
        }

        public static string GenerateInsertFor(DataTable table, ObjectMetadata meta, bool newLineBetweenColumns = true, bool newLineBetweenValues = false)
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

        public static string MockAllDependencies(System.Threading.CancellationToken token, TsqltManager.MockOptionsDictionary options, string connectionString, System.Collections.Generic.List<ObjectReference> dependencies)
        {
            var sb = new StringBuilder();
            QueryManager.Run(connectionString, token, (qm) =>
            {
                foreach (var m in dependencies)
                {
                    var objectType = GetObjectTypeDesc(qm, m);

                    if (objectType == null)
                    {
                        sb.AppendLine("-- Could not find type for " + m.ReferencedObject.ToFullString());
                    }
                    else if (objectType == "USER_TABLE")
                    {
                        sb.AppendLine(TsqltManager.MockTableWithRows(token, options, 1, m.ReferencedObject, connectionString));
                    }
                    else if (objectType == "SQL_STORED_PROCEDURE")
                    {
                        sb.AppendLine(TsqltManager.MockProcedure(m.ReferencedObject));
                    }
                    else if (objectType.ToUpperInvariant().Contains("FUNCTION"))
                    {
                        sb.AppendLine(TsqltManager.MockFunction(m.ReferencedObject));
                    }
                    else
                    {
                        sb.AppendLine("-- Unknown object type: " + m.ReferencedObject.ToFullString());
                    }
                    sb.AppendLine();
                }
            });
            return sb.ToString();
        }

        private static string GetObjectTypeDesc(QueryManager qm, ObjectReference m)
        {
            var objectType = qm.ExecuteScalar<string>(
                    string.Format(@"SELECT coalesce(type_desc, 'unknown')                                             FROM {0}.sys.objects o                                            INNER JOIN {0}.sys.schemas s ON s.schema_id = o.schema_id                                            WHERE o.name = '{1}' and s.name = '{2}'"
                    , m.ReferencedObject.DatabaseName
                    , m.ReferencedObject.ObjectName
                    , m.ReferencedObject.SchemaName));
            return objectType;
        }

        private static string MockFunction(ObjectMetadata meta)
        {
            return string.Format("EXEC {0}TSQLT.fakeFunction '{1}.{2}' ... ;",
                    meta.DatabaseName != null ? meta.DatabaseName + "." : "",
                    meta.SchemaName,
                    meta.ObjectName);
        }

        private static string MockProcedure(ObjectMetadata meta)
        {
            return string.Format("EXEC {0}TSQLT.SpyProcedure '{1}.{2}';",
                    meta.DatabaseName != null ? meta.DatabaseName + "." : "",
                    meta.SchemaName,
                    meta.ObjectName);
        }
    }
}
