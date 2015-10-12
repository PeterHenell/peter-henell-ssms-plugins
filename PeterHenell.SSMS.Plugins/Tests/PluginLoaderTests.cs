using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Plugins;

namespace PeterHenell.SSMS.Plugins.Tests
{
    
    [TestFixture]
    public class PluginLoaderTests
    {
        [Test]
        public void ShouldLoadPlugin()
        {
            var loader = new PluginLoader<ICommandPlugin>();
            loader.LoadPlugin(System.Environment.CurrentDirectory, "PeterHenell.SSMS.Plugins.dll");
            var instances = loader.CreateInstances();
            Assert.That(instances.Count, Is.GreaterThanOrEqualTo(1));
        }

    }

    public class TestPlugin : ICommandPlugin
    {
        public int SomeProp { get; set; }
    }
}
