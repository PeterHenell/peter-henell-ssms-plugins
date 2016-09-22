using PeterHenell.SSMS.Plugins.Plugins.Config;
using PeterHenell.SSMS.Plugins.Shell;
//using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Plugins
{

    public abstract class CommandPluginBase
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        public string Name { get; private set; }
        public string Caption { get; private set; }
        public string ShortcutBinding { get; private set; }
        public string MenuGroup { get; private set; }
        protected ShellManager ShellManager { get; private set; }
        public PluginConfiguration PluginOptions { get; set; }
        public PluginConfiguration SupportedOptions { get; set; }
        /// <summary>
        /// Gets or sets if this plugin should be visible from the menu.
        /// Can only altered during plugin load. Once loaded the property is no longer checked.
        /// </summary>
        public bool VisibleInMenu { get; set; }


        protected CommandPluginBase(string name, string menuGroup, string caption, string shortcutBinding, bool visibleInMenu = true)
        {
            this.Name = name;
            this.Caption = caption;
            this.ShortcutBinding = shortcutBinding;
            this.MenuGroup = menuGroup;
            this.cancellationTokenSource = new CancellationTokenSource();
            this.PluginOptions = new PluginConfiguration(caption);
            this.SupportedOptions = new PluginConfiguration(caption);
            this.VisibleInMenu = visibleInMenu;
        }

        public void Init(ShellManager shellManager)
        {
            this.ShellManager = shellManager;
        }

        /// <summary>
        /// Execute the main command of the plugin
        /// </summary>
        public void ExecuteCommand()
        {
            var token = this.cancellationTokenSource.Token;
            ExecuteCommand(token);
        }

        /// <summary>
        /// Plugin specific action
        /// </summary>
        /// <param name="token"></param>
        public abstract void ExecuteCommand(CancellationToken token);
        

        /// <summary>
        /// Try to abort the running plugin
        /// </summary>
        public void TryAbortCommand()
        {
            this.cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Options which will be saved when changed.
        /// </summary>
        

        /// <summary>
        /// Standard Menu Groups used to group commands into menus.
        /// </summary>
        public static class MenuGroups
        {
            public static string DataGeneration = "Data Generation";
            public static string TSQLTTools = "TSQLT Tools";
            public static string Liquibase = "Liquibase";
        }
    }
}
