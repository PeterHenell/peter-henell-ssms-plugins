using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Data.SqlClient;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [MSTest.TestClass]
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

        [MSTest.TestMethod]
        public void ShouldOutputFakeTableWithInserts()
        {

        }
    }
}
