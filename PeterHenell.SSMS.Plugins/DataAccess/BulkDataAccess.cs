using Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    public class BulkDataAccess
    {
        public void BulkInsertTo(IExcelDataReader excelReader, string tableName, ExcelSheetMetadata metadata)
        {
            using (var bulkCopy = new SqlBulkCopy(ConnectionManager.GetConnectionStringForCurrentWindow()))
            {
                bulkCopy.DestinationTableName = tableName;
                int ordinal = 0;
                foreach (var column in metadata.Columns)
                {
                    var mapping = new SqlBulkCopyColumnMapping
                    {
                        DestinationOrdinal = ordinal,
                        SourceOrdinal = ordinal++
                    };
                    bulkCopy.ColumnMappings.Add(mapping);
                }

                bulkCopy.WriteToServer(excelReader);
            }
        }
    }
}
