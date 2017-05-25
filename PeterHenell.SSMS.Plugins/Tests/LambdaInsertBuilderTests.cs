using FizzWare.NBuilder;
using NUnit.Framework;
using PeterHenell.SSMS.Plugins.QueryBuilders;
using PeterHenell.SSMS.Plugins.TypeBuilders;
using PeterHenell.SSMS.Plugins.Utils.Generators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    public class LambdaInsertBuilderTests
    {

        DataGenerator dataGenerator = new DataGenerator();

        [Test]
        public void ShouldGetInsertToTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            dataGenerator.Fill(dt, 10);

            var actual = LambdaInsertBuilder.Build((query) => query
                .Insert("Customer")
                .Columns(dt)
                .Values(dt)
                .End());

            var expected = @"INSERT INTO Customer ([ID], [Age], [Name])
VALUES 	(1, 1, 'Name1'), 
	(2, 2, 'Name2'), 
	(3, 3, 'Name3'), 
	(4, 4, 'Name4'), 
	(5, 5, 'Name5'), 
	(6, 6, 'Name6'), 
	(7, 7, 'Name7'), 
	(8, 8, 'Name8'), 
	(9, 9, 'Name9'), 
	(10, 10, 'Name10')";

            Console.WriteLine(actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldGetInsertFromProc()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            dataGenerator.Fill(dt, 10);

            var actual = LambdaInsertBuilder.Build((query) => query
                .Insert("Customer")
                .Columns(dt)
                .Query("EXEC GetCustomers")
                .End());

            var expected = @"INSERT INTO Customer ([ID], [Age], [Name])
EXEC GetCustomers";

            Console.WriteLine(actual);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
