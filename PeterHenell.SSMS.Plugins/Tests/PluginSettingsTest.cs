using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Plugins.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    public class PluginSettingsTest
    {
        [Test]
        public void ShouldLoadSettingsFromLocalStorage()
        {
            var config = new PluginConfiguration("testPlug");
            var fileName = "TestPluginFile";
            config.Add("FilePath", @"c:\src\git\");

            var manager = new PluginConfigurationManager();
            manager.Save(config);

            var loadedConfig = manager.Load(fileName);
            Assert.That(loadedConfig["FilePath"], Is.EqualTo(@"c:\src\git\"));
        }

        [Test]
        [Ignore("Running this test will show the windows form dialog.")]
        public void ShouldShowConfigWindow()
        {
            var configManager = new PluginConfigurationManager();
            var config = new PluginConfiguration("testPlug");
            config["Nyckel"] = "http://www.test.com";
            configManager.Configurations.Add(config);
            var pl = new ConfigurationPlugin { ConfigManager = configManager };
            
            pl.ExecuteCommand();

            foreach (var c in config.Keys)
            {
                Console.WriteLine("{0}: {1}", c, config[c]);
            }
        }
    }
}
