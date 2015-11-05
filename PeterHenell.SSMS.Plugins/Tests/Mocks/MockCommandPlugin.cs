using PeterHenell.SSMS.Plugins.Plugins;
using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins
{
    public class MockCommandPlugin : ICommandPlugin
    {
        private readonly string commandName;
        private readonly string menuGroup;

        public MockCommandPlugin()
        {
            commandName = "Mock";
            menuGroup = "Test";
        }

        public MockCommandPlugin(string commandName, string menuGroup)
        {
            this.commandName = commandName;
            this.menuGroup = menuGroup;
        }

        public string CommandName
        {
            get { return commandName; }
        }

        public string MenuGroup
        {
            get { return menuGroup; }
        }

        public void Init(RedGate.SIPFrameworkShared.ISsmsFunctionalityProvider4 provider)
        {
        }

        public string Caption
        {
            get { return "Mock"; }
        }

        public string[] DefaultBindings
        {
            get { return new[] { "global::Ctrl+Alt+Q" }; }
        }

        public bool Enabled
        {
            get { return false; }
        }

        public void Execute(object parameter)
        {
        }

        public RedGate.SIPFrameworkShared.ICommandImage Icon
        {
            get { return new CommandImageNone(); }
        }

        public string Name
        {
            get { return "Mock"; }
        }

        public string Tooltip
        {
            get { return "Mock"; }
        }

        public bool Visible
        {
            get { return false; }
        }
    }
}
