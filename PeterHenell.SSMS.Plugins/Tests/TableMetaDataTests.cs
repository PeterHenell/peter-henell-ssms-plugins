using NUnit.Framework;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    public class ObjectMetadataTests
    {
        [Test]
        public void ShouldGetFullyQualifiedNamesFromFullyQuailifiedObjectMetadata()
        {
            var tableA = ObjectMetadata.FromQualifiedString("MYDB.MySchema.MockingTable").ToFullString();
            var tableB = ObjectMetadata.FromQualifiedString("MySchema.MockingTable").ToFullString();
            var tableC = ObjectMetadata.FromQualifiedString("MockingTable").ToFullString();

            Assert.That(tableA, Is.EqualTo("[MYDB].[MySchema].[MockingTable]"));
            Assert.That(tableB, Is.EqualTo("[MySchema].[MockingTable]"));
            Assert.That(tableC, Is.EqualTo("[dbo].[MockingTable]"));
        }

        [Test]
        public void ShouldGetFullyQualifiedNamesFromObjectMetadata()
        {
            var tableA = ObjectMetadata.FromQualifiedString("[MYDB].[MySchema].[MockingTable]").ToFullString();
            var tableB = ObjectMetadata.FromQualifiedString("[MySchema].[MockingTable]").ToFullString();
            var tableC = ObjectMetadata.FromQualifiedString("[MockingTable]").ToFullString();

            Assert.That(tableA, Is.EqualTo("[MYDB].[MySchema].[MockingTable]"));
            Assert.That(tableB, Is.EqualTo("[MySchema].[MockingTable]"));
            Assert.That(tableC, Is.EqualTo("[dbo].[MockingTable]"));
        }

        [Test]
        public void ShouldUnwrapNames()
        {
            var tableA = ObjectMetadata.FromQualifiedString("[MYDB].[MySchema].[MockingTable]");

            Assert.That(tableA.ObjectName, Is.EqualTo("MockingTable"));
            Assert.That(tableA.SchemaName, Is.EqualTo("MySchema"));
            Assert.That(tableA.DatabaseName, Is.EqualTo("MYDB"));
        }
    }
}
