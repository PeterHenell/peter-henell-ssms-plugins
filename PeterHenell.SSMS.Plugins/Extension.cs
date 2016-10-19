using System;
using RedGate.SIPFrameworkShared;
using PeterHenell.SSMS.Plugins.Shell;
using System.Collections.Generic;
using PeterHenell.SSMS.Plugins.Utils;
using System.Windows.Forms;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Reflection;
using System.IO;
using EnvDTE80;
using PeterHenell.SSMS.Plugins.Plugins.Config;
using PeterHenell.SSMS.Plugins.Logging;

namespace PeterHenell.SSMS.Plugins
{
    public class Extension : ISsmsAddin
    {
        private ISsmsFunctionalityProvider4 _provider4;
        private DTE2 _Dte2;
        
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public void OnLoad(ISsmsExtendedFunctionalityProvider provider)
        {
            _provider4 = (ISsmsFunctionalityProvider4)provider;
            _Dte2 = _provider4.SsmsDte2 as DTE2;
            ISsmsExtendedFunctionalityProvider a;
            
            if (_provider4 == null)
                throw new ArgumentException();

            var debugWriter = new DebugTextWriter();
            Console.SetOut(debugWriter);
            
            try
            {
                LoadCommandPlugins();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void LoadCommandPlugins()
        {
            var formatter = new MenuFormatter();
            var pluginManager = new CommandPluginManager();

            try
            {
                pluginManager.LoadAllPlugins(Path.Combine(AssemblyDirectory, "Plugins"));
                var plugins = pluginManager.GetPluginInstances();
                
                // Add config plugin since it is a shared plugin
                SetupConfigurationPlugin(plugins);
              
                foreach (var plugin in plugins)
                {
                    plugin.Init(_provider4);
                    _provider4.AddGlobalCommand(plugin);
                }
                var menuGroups = formatter.GetMenuGroups(plugins);
                formatter.ConfigureMenu(menuGroups, _provider4);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        private static void SetupConfigurationPlugin(List<CommandPluginWrapper> plugins)
        {
            var configManager = new PluginConfigurationManager();
            configManager.LoadAll(plugins);
            var configPlugin = plugins.Find(x => x.Plugin.Name == ConfigurationPlugin.PLUGIN_NAME);
            ((ConfigurationPlugin)configPlugin.Plugin).ConfigManager = configManager;
        }

        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
            NodeManager.CurrentNode = node;
        }

        public string Version { get { return "Peter Henell SSMS Plugins 2015 1.1.0"; } }
        public string Author { get { return "Peter Henell"; } }
        public string URL { get { return "https://github.com/PeterHenell/peter-henell-ssms-plugins"; } }

    }
}