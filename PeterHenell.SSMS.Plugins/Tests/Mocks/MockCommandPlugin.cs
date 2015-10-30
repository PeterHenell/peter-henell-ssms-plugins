using PeterHenell.SSMS.Plugins.Plugins;
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
            throw new NotImplementedException();
        }

        public string Caption
        {
            get { throw new NotImplementedException(); }
        }

        public string[] DefaultBindings
        {
            get { throw new NotImplementedException(); }
        }

        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public RedGate.SIPFrameworkShared.ICommandImage Icon
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string Tooltip
        {
            get { throw new NotImplementedException(); }
        }

        public bool Visible
        {
            get { throw new NotImplementedException(); }
        }
    }
}
