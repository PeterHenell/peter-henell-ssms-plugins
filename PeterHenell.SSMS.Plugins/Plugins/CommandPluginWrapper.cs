using PeterHenell.SSMS.Plugins.Shell;
using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins
{
    /// <summary>
    /// This class wrapps the PluginCommandBase so that we do not need to reference the RedGate.SIPFrameworkShared 
    /// in the plugin projects.
    /// </summary>
    public class CommandPluginWrapper : ISharedCommandWithExecuteParameter
    {
        public string Name { get; private set; }
        public string Tooltip { get; private set; }
        public bool Visible { get { return true; } }
        public ISsmsFunctionalityProvider4 Provider { get; private set; }
        public RedGate.SIPFrameworkShared.ICommandImage Icon { get; private set; }
        public string Caption { get; private set; }
        public string[] DefaultBindings { get; private set; }
        public bool Enabled { get { return true; } }
        public string MenuGroup { get; private set; }

        public CommandPluginBase Plugin { get; private set; }

        public CommandPluginWrapper(CommandPluginBase loadedPlugin)
        {
            this.Plugin = loadedPlugin;

            this.Icon = new CommandImageNone();
            this.Name = loadedPlugin.Name;
            this.Caption = loadedPlugin.Caption;
            this.Tooltip = loadedPlugin.Caption;
            this.MenuGroup = loadedPlugin.MenuGroup;
            this.DefaultBindings = new string[] { loadedPlugin.ShortcutBinding };
        }

        public void Init(ISsmsFunctionalityProvider4 provider)
        {
            this.Provider = provider;
            var shellManager = new ShellManager(provider);
            Plugin.Init(shellManager);
        }

        public void Execute(object parameter)
        {
            try
            {
                Plugin.ExecuteCommand();
            }
            catch (System.Exception ex)
            {
                ShellManager.ShowMessageBox(ex.ToString());
            }
        }

        public void Execute()
        {
            try
            {
                Plugin.ExecuteCommand();
            }
            catch (System.Exception ex)
            {
                ShellManager.ShowMessageBox(ex.ToString());
            }
        }

        public override string ToString()
        {
            return string.Format("[{0} - {1} - {2}]", Name, Caption, MenuGroup);
        }
    }
}
