using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.Utils.Generators
{
    public class QueryBuilder
    {
        private readonly StringBuilder sb;

        public QueryBuilder()
        {
            this.sb = new StringBuilder();
        }

        public QueryBuilder SelectAllColumns(DataTable dt, bool newLines = false)
        {
            sb.Append("SELECT ");
            GetColumnList(dt, newLines);
            sb.AppendLine();

            return this;
        }

        public QueryBuilder GetColumnList(DataTable dt, bool newLines = false)
        {
            int columnCount = 1;
            foreach (DataColumn col in dt.Columns)
            {
                sb.AppendFormat("[{0}]", col.ColumnName);

                if (columnCount++ < dt.Columns.Count)
                    sb.Append(", ");
                if (newLines)
                    sb.AppendLine();
            }

            return this;
        }

        public QueryBuilder Into(ObjectMetadata targetTable)
        {
            sb.AppendLine("INTO " + targetTable.ToFullString());
            return this;
        }

        public QueryBuilder Where(string whereString)
        {
            sb.AppendLine("WHERE " + whereString);
            return this;
        }

        public string Build()
        {
            // construct and return the query
            // remove the last NewLine
            sb.Remove(sb.Length - 2, 2);
            if (sb[sb.Length - 1] != ';')
                sb.Append(";");
            
            return sb.ToString();
        }

        public QueryBuilder From(ObjectMetadata tableMeta)
        {
            sb.AppendLine("FROM " + tableMeta.ToFullString());
            return this;
        }

        public QueryBuilder MockTable(ObjectMetadata tableMeta)
        {
            var fakeTable = string.Format("EXEC {0}tSQLt.FakeTable '{1}{2}'",
                tableMeta.DatabaseName != null ? tableMeta.WrappedDatabaseName + "." : "",
                tableMeta.SchemaName != null ? tableMeta.WrappedSchemaName + "." : "",
                tableMeta.WrappedObjectName);

            sb.AppendLine(fakeTable);
            return this;
        }

        public QueryBuilder CreateTempTableFor(DataTable queryResult, ObjectMetadata tableMeta)
        {
            sb.AppendFormat("CREATE TABLE {0} (", tableMeta.ToFullString());
            sb.AppendLine();

            int columnCount = 1;
            foreach (DataColumn col in queryResult.Columns)
            {
                sb.AppendFormat("\t[{0}] {1}", col.ColumnName, DbTypeConverter.TranslateToSqlType(col.DataType).ToUpper());

                if (columnCount++ < queryResult.Columns.Count)
                    sb.Append(",");

                sb.AppendLine();
            }

            sb.Append(");");
            sb.AppendLine();
            return this;
        }

        public QueryBuilder InsertAllColumns(DataTable queryResult, ObjectMetadata tableMeta)
        {
            sb.Append("INSERT INTO " + tableMeta.ToFullString() + " (");
            GetColumnList(queryResult);
            sb.AppendLine(")");
            return this;
        }

        public QueryBuilder Exec(string query)
        {
            sb.AppendLine(query);
            return this;
        }
    }
}
