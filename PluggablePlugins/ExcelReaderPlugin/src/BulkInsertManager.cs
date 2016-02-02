using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderTest
{
    public class BulkInsertManager
    {

        public BulkInsertManager()
        {
            BatchSize = 1000;
        }

        public void BulkInsertTo(DataTable schema, string tableName, IDataReader dataReader, string connectionString)
        {
            using (var bulkCopy = new SqlBulkCopy(connectionString))
            {
                bulkCopy.BulkCopyTimeout = 9000000;
                bulkCopy.BatchSize = BatchSize;
                bulkCopy.DestinationTableName = string.Format("[{0}]", tableName);
                bulkCopy.EnableStreaming = true;
                
                for (int ordinal = 0; ordinal < schema.Columns.Count; ordinal++)
                {
                    var mapping = new SqlBulkCopyColumnMapping
                    {
                        DestinationOrdinal = ordinal,
                        SourceOrdinal = ordinal
                    };
                    bulkCopy.ColumnMappings.Add(mapping);
                }

                bulkCopy.WriteToServer(dataReader);
            }
        }

        public int BatchSize { get; set; }
    }
}
