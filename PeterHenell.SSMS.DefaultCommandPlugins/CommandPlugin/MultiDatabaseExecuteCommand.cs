using PeterHenell.SSMS.Plugins.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin
{
    public class MultiDatabaseExecuteCommand : CommandPluginBase
    {
        public MultiDatabaseExecuteCommand() :
            base("MultiDatabaseExecuteCommand", CommandPluginBase.MenuGroups.DataGeneration, "Execute Selected Query On Multiple Databases", "")
        {

        }

        public override void ExecuteCommand()
        {
            
        }
    }
}
