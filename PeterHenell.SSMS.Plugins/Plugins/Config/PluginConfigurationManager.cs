using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins.Config
{
    public class PluginConfigurationManager
    {
        private static readonly string configBaseDirectory;
        public List<PluginConfiguration> Configurations { get; set; }

        static PluginConfigurationManager()
        {
            //try
            //{
            //    var pathWithEnv = System.Configuration.ConfigurationManager.AppSettings["Plugins.Config.PluginManager.OptionsSaveFolder"];
            //    configBaseDirectory = Environment.ExpandEnvironmentVariables(pathWithEnv);
            //}
            //catch (Exception)
            //{
            //}

            if (string.IsNullOrEmpty(configBaseDirectory))
            {
                configBaseDirectory = ".";
            }
        }

        public PluginConfigurationManager()
        {
            this.Configurations = new List<PluginConfiguration>();
        }

        public void Save(PluginConfiguration config)
        {
            var configFilePath = Path.Combine(configBaseDirectory, config.OwnerName);
            BinarySerializer<PluginConfiguration>.WriteToFile(configFilePath, config, FileMode.Create);
        }

        public PluginConfiguration Load(string pluginName)
        {
            try
            {
                var configFilePath = Path.Combine(configBaseDirectory, pluginName);
                var obj = BinarySerializer<PluginConfiguration>.ReadFromFile(configFilePath);
                obj.OwnerName = pluginName;
                return obj;
            }
            catch (Exception ex)
            {
                // Probably did not the file exist, return an empty configuration
                Console.WriteLine(ex.ToString());
                return new PluginConfiguration(pluginName);
            }
        }

        public void SaveAll()
        {
            try
            {
                foreach (var config in Configurations)
                {
                    Save(config);
                }
            }
            catch (Exception)
            {
            }
        }

        public void LoadAll(List<CommandPluginWrapper> plugins)
        {
            try
            {
                foreach (var plugin in plugins)
                {
                    var config = this.Load(plugin.Caption);
                    if (config.Keys.Count == 0)
                    {
                        // If nothing is loaded, then use the plugin default configuration.
                        config = plugin.Plugin.SupportedOptions;
                    }
                    plugin.Plugin.PluginOptions = config;
                    Configurations.Add(config);
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public static class BinarySerializer<T>
    {
        public static void WriteToFile(string filePath, T objectToWrite, FileMode filemode)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (Stream stream = File.Open(filePath, filemode))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromFile(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
