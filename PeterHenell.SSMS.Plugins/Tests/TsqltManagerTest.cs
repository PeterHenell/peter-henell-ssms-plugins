using System;

using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Utils;
using PeterHenell.SSMS.Plugins.DataAccess;
using System.Data.SqlClient;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;

namespace PluginTests
{
    [TestFixture]
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

        [Test]
        public void ShouldOutputFakeTableWithInserts()
        {
            string selectedText = "MYDB.MySchema.MockingTable";
            var actual = TsqltManager.GetFakeTableStatement(selectedText);
            string expected = "EXEC [MYDB].tSQLt.FakeTable 'MySchema.MockingTable';";

            Assert.That(actual, Is.Not.Null.And.Not.Empty);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldOutputDefaultDatabaseWhenNoDatabaseSpecified()
        {
            string selectedText = "MySchema.MockingTable";
            var actual = TsqltManager.GetFakeTableStatement(selectedText);
            string expected = "EXEC tSQLt.FakeTable 'MySchema.MockingTable';";

            Assert.That(actual, Is.Not.Null.And.Not.Empty);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldOutputDefaultSchemaWhenNoSchemaSpecified()
        {
            string selectedText = "MockingTable";
            var actual = TsqltManager.GetFakeTableStatement(selectedText);
            string expected = "EXEC tSQLt.FakeTable 'dbo.MockingTable';";

            Assert.That(actual, Is.Not.Null.And.Not.Empty);
            Assert.That(actual, Is.EqualTo(expected));
        }



        [Test]
        [Ignore("")]
        public void ShouldGetTableDefinitionFromTableName()
        {
            var tableA = ObjectMetadata.FromQualifiedString("msdb.[dbo].[syscategories]");

            ObjectMetadataAccess da = new ObjectMetadataAccess(GetLocalConnection());
            var token = new CancellationTokenSource().Token;
            var actual = da.SelectTopNFrom(tableA, token);
            Assert.That(actual.Columns.Count, Is.EqualTo(4));
        }

        [Test]
        [Ignore("")]
        public void ShouldGenerateInsertIntoMockTable()
        {
            var meta = ObjectMetadata.FromQualifiedString("msdb.[dbo].[syscategories]");
            ObjectMetadataAccess da = new ObjectMetadataAccess(GetLocalConnection());
            var token = new CancellationTokenSource().Token;
            var table = da.SelectTopNFrom(meta, token);
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


        //[Test]
        //[ExpectedException(typeof(ArgumentException))]
        //public void ShouldThrowExceptionGivenBadInput()
        //{
        //    string selectedText = "";
        //    var res = TsqltManager.GetFakeTableStatement(selectedText);
        //}
    }
}
