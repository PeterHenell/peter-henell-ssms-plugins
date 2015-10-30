using NUnit.Framework;
using PeterHenell.SSMS.Plugins.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    public class DbTypeConverterTests
    {
        [Test]
        public void ShouldConvertToDbType()
        {
            var i = DbTypeConverter.TranslateToSqlType(typeof(int));
            var shrt = DbTypeConverter.TranslateToSqlType(typeof(short));
            var l = DbTypeConverter.TranslateToSqlType(typeof(long));
            var d = DbTypeConverter.TranslateToSqlType(typeof(decimal));
            var s = DbTypeConverter.TranslateToSqlType(typeof(string));
            var ba = DbTypeConverter.TranslateToSqlType(typeof(byte[]));
            var date = DbTypeConverter.TranslateToSqlType(typeof(DateTime));
            var flt = DbTypeConverter.TranslateToSqlType(typeof(float));
            var gid = DbTypeConverter.TranslateToSqlType(typeof(Guid));

            Assert.That(s.ToLower(), Is.EqualTo("nvarchar(max)"));
            Assert.That(i.ToLower(), Is.EqualTo("int"));
            Assert.That(shrt.ToLower(), Is.EqualTo("smallint"));
            Assert.That(l.ToLower(), Is.EqualTo("bigint"));
            Assert.That(d.ToLower(), Is.EqualTo("decimal(30,6)"));

            Assert.That(ba.ToLower(), Is.EqualTo("varbinary(max)"));
            Assert.That(date.ToLower(), Is.EqualTo("datetime2"));
            Assert.That(flt.ToLower(), Is.EqualTo("real"));
            Assert.That(gid.ToLower(), Is.EqualTo("uniqueidentifier"));
        }
    }
}
