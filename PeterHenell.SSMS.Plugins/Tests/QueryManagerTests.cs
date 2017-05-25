using System;

using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Utils;
using PeterHenell.SSMS.Plugins.DataAccess;
using System.Data.SqlClient;
using System.Threading;
using System.Data;

namespace PluginTests
{
    [TestFixture]
    public class QueryManagerTests
    {
        private string GetLocalConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"localhost\peheintegration";
            builder.InitialCatalog = "master";
            builder.IntegratedSecurity = true;
            return builder.ToString();
        }

        [Test]
        public void ShouldFillDataSetWithReturnValue()
        {
            string selectedText = "select * from sys.databases";
            var token = new CancellationTokenSource().Token;
            var ds = new DataSet();
            var count = QueryManager.Run(GetLocalConnection(), token, (qm) =>
            {
                qm.Fill(selectedText, ds);
                return ds.Tables.Count;
            });
            
            Assert.That(ds.Tables.Count, Is.EqualTo(1));
            Assert.That(count, Is.EqualTo(1));
        }
        [Test]
        public void ShouldFillDataSet()
        {
            string selectedText = "select * from sys.databases";
            var token = new CancellationTokenSource().Token;
            var ds = new DataSet();
            QueryManager.Run(GetLocalConnection(), token, (qm) =>
            {
                qm.Fill(selectedText, ds);
            });

            Assert.That(ds.Tables.Count, Is.EqualTo(1));
        }
       
    }
}
