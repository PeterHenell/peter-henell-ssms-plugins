using PeterHenell.SSMS.Plugins.Plugins.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins
{
    public class CommandPluginManager : PluginManager<CommandPluginBase>
    {
        public List<CommandPluginWrapper> GetPluginInstances()
        {
            var plugins = base.GetFilteredPluginInstances(i => IsValid(i))
                        .Select( p => new CommandPluginWrapper(p)).ToList();

            var configManager = new PluginConfigurationManager();
            foreach (var plugin in plugins)
            {
                var options = configManager.Load(plugin.Name);
                plugin.Options = options;
            }
            
            return plugins;
        }

        private bool IsValid(CommandPluginBase i)
        {
            return 
                   i.MenuGroup != null
                && i.Name != null
                && i.Caption != null
                && i.ShortcutBinding != null;
        }
    }
}
