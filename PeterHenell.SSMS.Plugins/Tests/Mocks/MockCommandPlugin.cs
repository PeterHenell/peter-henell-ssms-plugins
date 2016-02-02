using PeterHenell.SSMS.Plugins.Plugins;
using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins
{
    public class MockCommandPlugin : CommandPluginBase
    {
        private readonly string commandName;
        private readonly string menuGroup;

        public MockCommandPlugin() :
            base("Mock",
                 "Test",
                 "Generate Insert statement for selected query",
                 "global::Ctrl+Alt+G")
        {
        }

        public MockCommandPlugin(string commandName, string menuGroup) :
            base(commandName,
                 menuGroup,
                 "Generate Insert statement for selected query",
                 "global::Ctrl+Alt+G")
        {
            this.commandName = commandName;
            this.menuGroup = menuGroup;
        }

        public override void ExecuteCommand()
        {
            
        }
    }
}
