using PeterHenell.SSMS.Plugins.Shell;
//using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Plugins
{

    public abstract class CommandPluginBase
    {
        public string Name { get; private set; }
        public string Caption { get; private set; }
        public string ShortcutBinding { get; private set; }
        public string MenuGroup { get; private set; }
        protected ShellManager ShellManager { get; private set; }

        public CommandPluginBase(string name, string menuGroup, string caption, string shortcutBinding)
        {
            this.Name = name;
            this.Caption = caption;
            this.ShortcutBinding = shortcutBinding;
            this.MenuGroup = menuGroup;
        }

        public void Init(ShellManager shellManager)
        {
            this.ShellManager = shellManager;
        }

        public abstract void ExecuteCommand();

        /// <summary>
        /// Standard Menu Groups used to group commands into menus.
        /// </summary>
        public static class MenuGroups
        {
            public static string DataGeneration = "Data Generation";
            public static string TSQLTTools = "TSQLT Tools";
        }
    }
}
