using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;
using PeterHenell.SSMS.Plugins.Utils.Generators;

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
                        sb.AppendLine(TsqltManager.MockProcedure(m.ReferencedObject, qm));
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

        private static string MockProcedure(ObjectMetadata meta, QueryManager qm)
        {
            var sb = new StringBuilder();
//            var qb = new QueryBuilder();
//            var sql = string.Format(@"SET FMTONLY ON 
//                        {0}
//                        SET FMTONLY OFF ", GetExecutionOfProcedureString(meta, qm));
//            var dt = new DataTable();
//            Console.WriteLine(sql);
//            qm.Fill(sql, dt);

//            sb.AppendLine(string.Format("CREATE PROCEDURE MOCKING.{0}_{1} AS", meta.SchemaName, meta.ObjectName));
//            sb.AppendLine(qb.SelectAllColumns(dt, true).Build());
//            sb.AppendLine("GO");

            return string.Format("EXEC {0}TSQLT.SpyProcedure '{1}.{2}';",
                    meta.DatabaseName != null ? meta.DatabaseName + "." : "",
                    meta.SchemaName,
                    meta.ObjectName);
        }

        private static string GetExecutionOfProcedureString(ObjectMetadata meta, QueryManager qm)
        {
            var sql = string.Format(@"WITH paramValues(DataTyp, VALUE) AS(
                    SELECT 'uniqueidentifier', 'A8ABE1E2-90B9-40F6-9A23-4160D47B275F' UNION ALL 
                    SELECT 'date', '2012-10-08' UNION ALL 
                    SELECT 'time', '10:14:26.033' UNION ALL 
                    SELECT 'datetime2', '2012-10-08 10:14:26.033' UNION ALL 
                    SELECT 'tinyint', '1' UNION ALL 
                    SELECT 'smallint', '1' UNION ALL 
                    SELECT 'int', '1' UNION ALL 
                    SELECT 'smalldatetime', '2012-10-08' UNION ALL 
                    SELECT 'datetime', '2012-10-08 10:14:26.033' UNION ALL 
                    SELECT 'float', '1' UNION ALL 
                    SELECT 'ntext', 'a' UNION ALL 
                    SELECT 'bit', '1' UNION ALL 
                    SELECT 'decimal', '1' UNION ALL 
                    SELECT 'numeric', '1' UNION ALL 
                    SELECT 'bigint', '1' UNION ALL 
                    SELECT 'varbinary', '0x7065746572' UNION ALL 
                    SELECT 'varchar', 'a' UNION ALL 
                    SELECT 'char', 'a' UNION ALL 
                    SELECT 'nvarchar', 'a' UNION ALL 
                    SELECT 'nchar', 'a' UNION ALL 
                    SELECT 'xml', '<peter>peter</peter>' UNION ALL 
                    SELECT 'sysname', 'object'
                )

                SELECT 
                   'EXEC {2}.{0}.{1} ' + CASE WHEN paramAndValue IS NOT NULL THEN ' @' + STUFF( SUBSTRING(paramAndValue, 0, LEN(paramAndValue)), 1, 1, '') + ';'
                                                ELSE ';'
                                            END
                FROM 
                    {2}.sys.objects ob
                OUTER APPLY
                (
                    SELECT 
                        pa.name + ' = ' + '''' + pv.value + '''' + ', '
                    FROM
                        {2}.sys.parameters pa
                    INNER JOIN
                        paramValues pv
                        ON pv.DataTyp = TYPE_NAME(pa.system_type_id)
                    WHERE 
                        ob.object_id = pa.object_id
                    ORDER BY pa.object_id    
                    FOR XML PATH('')
        
                ) pa(paramAndValue)
    
                WHERE 
                    TYPE = 'p' AND ob.object_id = OBJECT_ID('{2}.{0}.{1}')", meta.SchemaName, meta.ObjectName, meta.DatabaseName);
            
            var dt = new DataTable();
            qm.Fill(sql, dt);
            if (dt.Rows.Count > 0)
            {
                var res = dt.Rows[0].Field<string>(0);
                return res;
            }
            else
            {
                return string.Format("EXEC {2}.{0}.{1}", meta.SchemaName, meta.ObjectName, meta.DatabaseName);
            }
            

        }
    }
}
