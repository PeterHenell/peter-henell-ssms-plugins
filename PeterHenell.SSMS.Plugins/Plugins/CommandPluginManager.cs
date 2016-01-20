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
            return base.GetFilteredPluginInstances(i => IsValid(i))
                        .Select( p => new CommandPluginWrapper(p)).ToList();
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
