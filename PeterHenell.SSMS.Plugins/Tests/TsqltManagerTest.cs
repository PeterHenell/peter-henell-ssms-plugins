using System;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Utils;
using PeterHenell.SSMS.Plugins.DataAccess;
using System.Data.SqlClient;

namespace PluginTests
{
    [MSTest.TestClass]
    public class TsqltManagerTest
    {
        private string GetLocalConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.InitialCatalog = "master";
            builder.IntegratedSecurity = true;
            return builder.ToString();
        }

        [MSTest.TestMethod]
        public void ShouldOutputFakeTableWithInserts()
        {
            string selectedText = "MYDB.MySchema.MockingTable";
            var actual = TsqltManager.GetFakeTableStatement(selectedText);
            string expected = "EXEC [MYDB].tSQLt.FakeTable 'MySchema.MockingTable';";

            Assert.That(actual, Is.Not.Null.And.Not.Empty);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [MSTest.TestMethod]
        public void ShouldOutputDefaultDatabaseWhenNoDatabaseSpecified()
        {
            string selectedText = "MySchema.MockingTable";
            var actual = TsqltManager.GetFakeTableStatement(selectedText);
            string expected = "EXEC tSQLt.FakeTable 'MySchema.MockingTable';";

            Assert.That(actual, Is.Not.Null.And.Not.Empty);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [MSTest.TestMethod]
        public void ShouldOutputDefaultSchemaWhenNoSchemaSpecified()
        {
            string selectedText = "MockingTable";
            var actual = TsqltManager.GetFakeTableStatement(selectedText);
            string expected = "EXEC tSQLt.FakeTable 'dbo.MockingTable';";

            Assert.That(actual, Is.Not.Null.And.Not.Empty);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [MSTest.TestMethod]
        public void ShouldGetFullyQualifiedNamesFromFullyQuailifiedTableMetaData()
        {
            var tableA = TableMetadata.FromQualifiedString("MYDB.MySchema.MockingTable").ToFullString();
            var tableB = TableMetadata.FromQualifiedString("MySchema.MockingTable").ToFullString();
            var tableC = TableMetadata.FromQualifiedString("MockingTable").ToFullString();

            Assert.That(tableA, Is.EqualTo("[MYDB].[MySchema].[MockingTable]"));
            Assert.That(tableB, Is.EqualTo("[MySchema].[MockingTable]"));
            Assert.That(tableC, Is.EqualTo("[dbo].[MockingTable]"));
        }

        [MSTest.TestMethod]
        public void ShouldGetFullyQualifiedNamesFromTableMetaData()
        {
            var tableA = TableMetadata.FromQualifiedString("[MYDB].[MySchema].[MockingTable]").ToFullString();
            var tableB = TableMetadata.FromQualifiedString("[MySchema].[MockingTable]").ToFullString();
            var tableC = TableMetadata.FromQualifiedString("[MockingTable]").ToFullString();

            Assert.That(tableA, Is.EqualTo("[MYDB].[MySchema].[MockingTable]"));
            Assert.That(tableB, Is.EqualTo("[MySchema].[MockingTable]"));
            Assert.That(tableC, Is.EqualTo("[dbo].[MockingTable]"));
        }

        [MSTest.TestMethod]
        public void ShouldUnwrapNames()
        {
            var tableA = TableMetadata.FromQualifiedString("[MYDB].[MySchema].[MockingTable]");

            Assert.That(tableA.TableName, Is.EqualTo("MockingTable"));
            Assert.That(tableA.SchemaName, Is.EqualTo("MySchema"));
            Assert.That(tableA.DatabaseName, Is.EqualTo("MYDB"));
        }

        [MSTest.TestMethod]
        public void ShouldGetTableDefinitionFromTableName()
        {
            var tableA = TableMetadata.FromQualifiedString("msdb.[dbo].[syscategories]");

            TableMetaDataAccess da = new TableMetaDataAccess(GetLocalConnection());

            var actual = da.SelectTopNFrom(tableA);
            Assert.That(actual.Columns.Count, Is.EqualTo(4));
        }

        [MSTest.TestMethod]
        public void ShouldGenerateInsertIntoMockTable()
        {
            var meta = TableMetadata.FromQualifiedString("msdb.[dbo].[syscategories]");
            TableMetaDataAccess da = new TableMetaDataAccess(GetLocalConnection());

            var table = da.SelectTopNFrom(meta);
            var actual = TsqltManager.GenerateInsertFor(table, meta);

            var expected = @"INSERT INTO [msdb].[dbo].[syscategories] (
	[category_id],
	[category_class],
	[category_type],
	[name]
)
" + "VALUES\t(0, 1, 1, '[Uncategorized (Local)]');";

            Console.WriteLine(expected);
            Console.WriteLine(actual);

            Assert.That(actual, Is.EqualTo(expected));
        }


        [MSTest.TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowExceptionGivenBadInput()
        {
            string selectedText = "";
            var res = TsqltManager.GetFakeTableStatement(selectedText);
        }
    }
}
