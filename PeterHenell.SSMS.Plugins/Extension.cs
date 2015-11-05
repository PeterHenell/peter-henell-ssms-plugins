using System;
using RedGate.SIPFrameworkShared;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Commands;
using System.Collections.Generic;
using PeterHenell.SSMS.Plugins.Utils;
using System.Windows.Forms;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Reflection;
using System.IO;
using EnvDTE80;

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

            if (_provider4 == null)
                throw new ArgumentException();

            LoadCommandPlugins();
        }

        private void LoadCommandPlugins()
        {
            MenuFormatter formatter = new MenuFormatter();
            var pluginManager = new PluginManager<ICommandPlugin>();

            try
            {
                pluginManager.LoadAllPlugins(AssemblyDirectory);
                var plugins = pluginManager.GetPluginInstances(i => i.Enabled);
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

        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
            NodeManager.CurrentNode = node;
        }

        public string Version { get { return "Peter Henell SSMS Plugins 2015 1.1.0"; } }
        public string Author { get { return "Peter Henell"; } }
        public string URL { get { return "https://github.com/PeterHenell/peter-henell-ssms-plugins"; } }

    }
}