using NUnit.Framework;
using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    public class ObjectDependencyServiceTests
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
        public void ShouldGetObjectDependencies()
        {
            var token = new CancellationTokenSource().Token;
            var service = new ObjectDependencyService();
            var meta =  ObjectMetadata.FromQualifiedString("ETL.fillCountry");
            
            var result = service.GetDependencies(meta, GetLocalConnection(), token);
        }

        [Test]
        public void ShouldMockAllObjectDependencies()
        {
            var token = new CancellationTokenSource().Token;
            var service = new ObjectDependencyService();
            var options = new PeterHenell.SSMS.Plugins.Utils.TsqltManager.MockOptionsDictionary();
            options.EachColumnInSelectOnNewRow = false;
            options.EachColumnInValuesOnNewRow = false;


            var meta = ObjectMetadata.FromQualifiedString("ETL.fillCountry");
            var deps = service.GetDependencies(meta, GetLocalConnection(), token);
            
            var res = TsqltManager.MockAllDependencies(token, options, GetLocalConnection(), deps);
            Console.WriteLine(res.ToString());
        }

    }
}
