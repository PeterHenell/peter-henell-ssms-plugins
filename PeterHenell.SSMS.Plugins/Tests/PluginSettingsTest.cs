using NUnit.Framework;
using PeterHenell.SSMS.Plugins.Plugins.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PeterHenell.SSMS.Plugins.Tests
{
    [TestFixture]
    public class PluginSettingsTest
    {

        

        //public class PluginConfiguration
        //{
        //    private Dictionary<string, string> configValues;

        //    private PluginConfiguration(Dictionary<string, string> dict)
        //    {
        //        this.configValues = new Dictionary<string, string>();
        //        foreach (var k in dict.Keys)
        //        {
        //            configValues[k] = dict[k];
        //        }
        //    }

        //    public void SetValue(string key, string value)
        //    {
        //        configValues[key] = value;
        //    }

        //    public static PluginConfiguration FromDictionary(Dictionary<string, string> dict)
        //    {
        //        return new PluginConfiguration(dict);
        //    }

        //}

        


        [Test]
        public void ShouldLoadSettingsFromLocalStorage()
        {
            var config = new PluginConfiguration();
            var fileName = "TestPluginFile";
            config.Add("FilePath", @"c:\src\git\");

            var manager = new PluginConfigurationManager();
            manager.Save(fileName, config);

            var loadedConfig = manager.Load(fileName);
            Assert.That(loadedConfig["FilePath"], Is.EqualTo(@"c:\src\git\"));
        }
    }
}
