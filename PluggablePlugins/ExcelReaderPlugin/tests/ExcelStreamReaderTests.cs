using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderTest
{
    [TestFixture]
    public class ExcelStreamReaderTests
    {
        string fileName = @"C:\Users\pehe\Downloads\REPORT 73.xlsx";

        [Test]
        public void ShouldReadHeaders()
        {
            ExcelStreamReader.Execute(fileName, reader =>

                reader.ForEachSheet(sheet =>
                {
                    sheet.ForEachRow((schema, row) =>
                    {
                        Console.WriteLine("New Row");
                        for (int i = 0; i < schema.Columns.Count; i++)
                        {
                            Console.WriteLine(row[i]);
                        }
                    });
                })
            );
        }

        [Test]
        public void ShouldReadAllRows()
        {
            ExcelStreamReader.Execute(fileName, reader =>

                reader.ForEachSheet(sheet =>
                {
                    var schema = sheet.GetSchema();

                    foreach (var row in sheet.GetRows())
                    {
                        Console.WriteLine("New Row");
                        for (int i = 0; i < schema.Columns.Count; i++)
                        {
                            Console.WriteLine(row[i]);
                        }
                    }
                })
            );
        }

        [Test]
        public void ImportAllRows()
        {
            var ddlManager = new DDLManager();
            var bulkdInsertManager = new BulkInsertManager();
            String connString = GetConnectionString();
            Console.WriteLine("Start time" + DateTime.Now);
            
            ExcelStreamReader.Execute(fileName, reader =>

                reader.ForEachSheet(sheet =>
                {
                    var schema = sheet.GetSchema();

                    ddlManager.CreateTable(schema.TableName, schema, connString);
                    bulkdInsertManager.BulkInsertTo(schema, schema.TableName, sheet, connString);
                })
            );
            
            Console.WriteLine("End time" + DateTime.Now);
        }

        private string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.IntegratedSecurity = true;
            builder.InitialCatalog = "testdb";
            return builder.ToString();
        }
    }
}
