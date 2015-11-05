using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins
{
    public class CommandPluginManager : PluginManager<ICommandPlugin>
    {
        public List<ICommandPlugin> GetPluginInstances()
        {
            return base.GetFilteredPluginInstances(i => IsValid(i));
        }

        private bool IsValid(ICommandPlugin i)
        {
            return i.Enabled
                && i.Icon != null
                && i.MenuGroup != null
                && i.Name != null
                && i.Caption != null
                && i.DefaultBindings != null
                && i.Tooltip != null;
        }
    }
}
