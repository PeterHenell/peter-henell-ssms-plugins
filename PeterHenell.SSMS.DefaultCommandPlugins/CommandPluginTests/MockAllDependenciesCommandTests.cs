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
            builder.InitialCatalog = "DWH_PRES";
            builder.IntegratedSecurity = true;
            return builder.ToString();
        }

       
        [Test]
        public void ShouldMockAllDependenciesForProcedure()
        {
            string selectedText = "ETL.fillCounterpart";
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
