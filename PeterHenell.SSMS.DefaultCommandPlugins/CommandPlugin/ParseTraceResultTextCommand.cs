using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Plugins;
using PeterHenell.SSMS.Plugins.Shell;
using System;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ParseTraceResultText_Command : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "ParseTraceResultText_Command";

        public ParseTraceResultText_Command() :
            base(COMMAND_NAME,
                  CommandPluginBase.MenuGroups.DataGeneration,
                  "Parse Trace Results",
                  "")
        {

        }

        public override void ExecuteCommand()
        {
            // implement command here
        }


        //public string Name { get { return COMMAND_NAME ; } }
        //public string Caption { get { return "Parse Trace Results"; } }
        //public string Tooltip { get { return "."; }}
        //public ICommandImage Icon { get { return m_CommandImage; } }
        //public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+D" }; } }
        //public bool Visible { get { return true; } }
        //public bool Enabled { get { return true; } }

        //public void Execute()
        //{
            
        //}

        //public string MenuGroup
        //{
        //    get { return "Data Generation"; }
        //}

        //public void Init(ISsmsFunctionalityProvider4 provider)
        //{
        //    this.provider = provider;
        //    this.shellManager = new ShellManager(provider);
        //}
    }
}