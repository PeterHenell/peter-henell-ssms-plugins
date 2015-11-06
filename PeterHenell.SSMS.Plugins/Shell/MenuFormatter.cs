using EnvDTE80;
using PeterHenell.SSMS.Plugins.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Shell
{
    public class MenuFormatter
    {
        public MenuFormatter()
        {
        }
        /// <summary>
        /// Format ICommandPlugins into groups based on the MenuGroup property
        /// </summary>
        /// <param name="plugins"></param>
        /// <returns></returns>
        public Dictionary<string, List<ICommandPlugin>> GetMenuGroups(List<ICommandPlugin> plugins)
        {
            var groups = plugins.GroupBy(x => x.MenuGroup)
                 .ToDictionary(g => g.Key, g => g.ToList());

            return groups;
        }

        internal void ConfigureMenu(Dictionary<string, List<ICommandPlugin>> menuGroups, RedGate.SIPFrameworkShared.ISsmsFunctionalityProvider4 _provider4)
        {
            //var commandbar = ((DTE2)_provider4.SsmsDte2).CommandBars["Peter Henell"] as Microsoft.VisualStudio.CommandBars.CommandBars;
            //commandbar.ActiveMenuBar.Delete();
            var peterhenell = _provider4.MenuBar.MainMenu.BeginSubmenu("Peter Henell", "Peter Henell");
            foreach (var groupName in menuGroups.Keys)
            {
                var sub = peterhenell.BeginSubmenu(groupName, groupName);
                foreach (var command in menuGroups[groupName].OrderBy(x => x.Caption))
                {
                    sub.AddCommand(command.Name);
                }
                sub.EndSubmenu();
            }

            peterhenell.EndSubmenu();
        }
    }
}
