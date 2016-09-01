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

        static PluginConfigurationManager()
        {
            var pathWithEnv = System.Configuration.ConfigurationManager.AppSettings["Plugins.Config.PluginManager.OptionsSaveFolder"];
            configBaseDirectory = Environment.ExpandEnvironmentVariables(pathWithEnv);
        }

        public PluginConfigurationManager()
        {
        }

        public void Save(string configFileName, PluginConfiguration config)
        {
            var configFilePath = Path.Combine(configBaseDirectory, configFileName);
            BinarySerializer<PluginConfiguration>.WriteToFile(configFilePath, config, FileMode.Create);
        }

        public PluginConfiguration Load(string configFileName)
        {
            try
            {
                var configFilePath = Path.Combine(configBaseDirectory, configFileName);
                var obj = BinarySerializer<PluginConfiguration>.ReadFromFile(configFilePath);
                return obj;
            }
            catch (Exception ex)
            {
                // Probably did not the file exist, return an empty configuration
                Console.WriteLine(ex.ToString());
                return new PluginConfiguration();
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
