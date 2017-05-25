using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin.DataSetComparer;
using System.Threading;
using System.Data.SqlClient;

namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPluginTests.DataSetCompareCommandTests
{
    [TestFixture]
    public class DataSetComparerTest
    {
        DataSetComparerCommand command = new DataSetComparerCommand();

        [Test]
        public void DatasetShouldBeEqualToItself()
        {
            var token = CancellationToken.None;
            var result = command.GetComparisonResultFrom(ref token, "select * from sys.databases select * from sys.databases", GetConnectionString());
            Console.WriteLine(result);
        }

        private string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.IntegratedSecurity = true;
            builder.InitialCatalog = "master";
            builder.DataSource = "pehe-764\\peheintegration";
            return builder.ToString();
        }

    }
}
