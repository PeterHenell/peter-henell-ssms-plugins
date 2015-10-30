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
        /// <summary>
        /// Format ICommandPlugins into groups based on the MenuGroup property
        /// </summary>
        /// <param name="plugins"></param>
        /// <returns></returns>
        public Dictionary<string, List<ICommandPlugin>> GetMenuGroups(List<ICommandPlugin> plugins)
        {
            var groups = plugins.GroupBy(x => x.MenuGroup)
                 .ToDictionary(g => g.Key, g => g.ToList()); 

            Console.WriteLine(groups.Count);

            return groups;
        }
    }
}
