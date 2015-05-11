using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class TableMetadata
    {
        public string TableName { get; private set; }
        public string SchemaName { get; private set; }
        public string DatabaseName { get; private set; }

        public string WrappedTableName { get { return Wrap(TableName); } }
        public string WrappedSchemaName { get { return Wrap(SchemaName); } }
        public string WrappedDatabaseName { get { return Wrap(DatabaseName); } }

        private TableMetadata()
        {
        }

        public static TableMetadata FromQualifiedString(String qualifiedString)
        {
            string[] parts = qualifiedString.Split('.');
            return FromParts(parts);
        }

        public static TableMetadata FromParts(String[] parts)
        {
            if (parts.Length < 1)
            {
                throw new ArgumentException("Selected text does not contain any table name");
            }

            int numPartCounter = 0;
            string dbName = parts.Length == 3 ? parts[numPartCounter++] : null;
            string schemaName = parts.Length >= 2 ? parts[numPartCounter++] : "dbo";
            string tableName = parts[numPartCounter];

            return new TableMetadata { TableName = UnWrap(tableName), SchemaName = UnWrap(schemaName), DatabaseName = UnWrap(dbName) };
        }

        private static string UnWrap(string s)
        {
            if (s == null)
            {
                return null;
            }
            return s.Trim().Replace("[", "").Replace("]", "");
        }

        public string ToFullString()
        {
            return string.Format("{0}{1}{2}",
                   DatabaseName != null ? WrappedDatabaseName + "." : "",
                   SchemaName != null ? WrappedSchemaName + "." : "dbo.",
                   WrappedTableName
                   );
        }

        private string Wrap(string s)
        {
            if (s ==  null)
            {
                return null;
            }
            if (!s.StartsWith("["))
            {
                s = "[" + s + "]";
            }
            return s;
        }
    }
}
