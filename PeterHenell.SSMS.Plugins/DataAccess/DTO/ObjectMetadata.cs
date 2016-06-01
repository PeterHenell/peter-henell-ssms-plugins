using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.DataAccess.DTO
{
    public class ObjectMetadata
    {
        public string ObjectName { get; private set; }
        public string SchemaName { get; private set; }
        public string DatabaseName { get; private set; }

        public string WrappedObjectName { get { return Wrap(ObjectName); } }
        public string WrappedSchemaName { get { return Wrap(SchemaName); } }
        public string WrappedDatabaseName { get { return Wrap(DatabaseName); } }

        private ObjectMetadata()
        {
        }

        public static ObjectMetadata FromQualifiedString(String qualifiedString)
        {
            string[] parts = qualifiedString.Split('.');
            return FromParts(parts);
        }
        public static ObjectMetadata FromParts(string objectName)
        {
            return new ObjectMetadata { ObjectName = UnWrap(objectName), SchemaName = UnWrap("dbo"), DatabaseName = null };
        }
        public static ObjectMetadata FromParts(string schemaName, string objectName)
        {
            return new ObjectMetadata { ObjectName = UnWrap(objectName), SchemaName = UnWrap(schemaName), DatabaseName = null };
        }
        public static ObjectMetadata FromParts(string dbName, string schemaName, string objectName)
        {
            return new ObjectMetadata { ObjectName = UnWrap(objectName), SchemaName = UnWrap(schemaName), DatabaseName = UnWrap(dbName) };
        }

        public static ObjectMetadata FromParts(String[] parts)
        {
            if (parts.Length < 1)
            {
                throw new ArgumentException("Selected text does not contain any object name");
            }

            int numPartCounter = 0;
            string dbName = parts.Length == 3 ? parts[numPartCounter++] : null;
            string schemaName = parts.Length >= 2 ? parts[numPartCounter++] : "dbo";
            string objectName = parts[numPartCounter];

            return new ObjectMetadata { ObjectName = UnWrap(objectName), SchemaName = UnWrap(schemaName), DatabaseName = UnWrap(dbName) };
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
                   WrappedObjectName
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
