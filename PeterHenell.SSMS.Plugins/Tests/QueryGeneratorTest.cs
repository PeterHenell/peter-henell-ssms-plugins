
using NUnit.Framework;
using System.Data.SqlClient;
using System.Data;
using PeterHenell.SSMS.Plugins.Utils;
using PeterHenell.SSMS.Plugins.Utils.Generators;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    public class QueryGeneratorTest
    {
        private string GetLocalConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.InitialCatalog = "master";
            builder.IntegratedSecurity = true;
            return builder.ToString();
        }

        [Test]
        public void ShouldTypeFluentlyToGetGeneratedQueries()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn { ColumnName = "CustomerID", DataType = typeof(long) });
            dt.Columns.Add(new DataColumn { ColumnName = "Name", DataType = typeof(string) });

            var tableMeta = TableMetadata.FromQualifiedString("tempdb.dbo.#Actual");
            var targetTable = TableMetadata.FromQualifiedString("tempdb.dbo.#Excpected");
            
            var queryGen = new QueryBuilder();
            string expected = @"SELECT [CustomerID], [Name]
INTO [tempdb].[dbo].[#Excpected]
FROM [tempdb].[dbo].[#Actual]
WHERE 1=0;";
            string query = queryGen.SelectAllColumns(dt).Into(targetTable).From(tableMeta).Where("1=0").Build();
            Assert.That(query, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldOutputFakeTableWithInserts()
        {
            var tableMeta = TableMetadata.FromQualifiedString("dbo.Customer");
            var queryGen = new QueryBuilder();
            var actual = queryGen.MockTable(tableMeta).Build();

            string expected = @"EXEC tSQLt.FakeTable '[dbo].[Customer]';";
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldInsertFromProcedureResult()
        {
            var selectedQuery = "SELECT * FROM Customer;";
            var queryResult = new DataTable();
            queryResult.Columns.Add(new DataColumn { ColumnName = "CustomerID", DataType = typeof(long) });
            queryResult.Columns.Add(new DataColumn { ColumnName = "Name", DataType = typeof(string) });

            var row = queryResult.NewRow();
            row.SetField(queryResult.Columns[0], 150L);
            row.SetField(queryResult.Columns[1], "Skogshuggaren");
            queryResult.Rows.Add(row);

            var targetTable = TableMetadata.FromQualifiedString("tempdb.dbo.#Actual");

            var queryGen = new QueryBuilder();
            var actual = queryGen.CreateTempTableFor(queryResult, targetTable)
                                 .InsertAllColumns(queryResult, targetTable)
                                 .Exec(selectedQuery)
                                 .Build();

            string expected = @"CREATE TABLE [tempdb].[dbo].[#Actual] (
" + "\t" + @"[CustomerID] BIGINT,
" + "\t" + @"[Name] NVARCHAR(MAX)
);
INSERT INTO [tempdb].[dbo].[#Actual] ([CustomerID], [Name])
" + selectedQuery;
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
