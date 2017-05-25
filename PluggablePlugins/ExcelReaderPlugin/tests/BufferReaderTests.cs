using ExcelReaderTest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderTests
{
    [TestFixture]
    public class BufferReaderTests
    {
        [Test]
        public void ShouldConsumeAllIEnumerables()
        {
            var br = new BufferReader<string>();
            var strings = new String[] { "Peter", "Have", "A", "Buffering", "Reader" };

            br.BeginRead(strings);
            var count = 0;
            while (!br.AllIsRead)
            {
                if (br.Pop() != null)
                {
                    count++;
                }
            }
            Assert.That(count, Is.EqualTo(5));
        }
    }
}
