using Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    public class ExcelImporter
    {
        public ExcelSheetMetadata GetMetaData(string file)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                var excelMetaData = new ExcelSheetMetadata();
                excelReader.IsFirstRowAsColumnNames = true;

                if (excelReader.Read())
                {
                    for (int i = 0; i < excelReader.FieldCount; i++)
                    {
                        var excelFieldName = excelReader.GetString(i);
                        if (string.IsNullOrEmpty(excelFieldName))
                        {
                            // Ignore blank column names
                            continue;
                        }

                        var columnName = excelFieldName.Trim();
                        excelMetaData.AddColumn(columnName);
                    }
                }

                return excelMetaData;
            }
        }

        public void ImportToTable(string path, string tableName, ExcelSheetMetadata meta)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                excelReader.IsFirstRowAsColumnNames = true;
                excelReader.Read();
                var bulk = new BulkDataAccess();
                bulk.BulkInsertTo(excelReader, tableName, meta);
            }
        }
    }
}
