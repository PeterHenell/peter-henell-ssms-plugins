using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Plugins;
using PeterHenell.SSMS.Plugins.Shell;
using RedGate.SIPFrameworkShared;
using System;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ParseTraceResultText_Command : ICommandPlugin
    {
        public readonly static string COMMAND_NAME = "ParseTraceResultText_Command";
        
        private ISsmsFunctionalityProvider4 provider;
        ShellManager shellManager;
        private readonly ICommandImage m_CommandImage = new CommandImageNone();


        public void Execute(object parameter)
        {
            try
            {
                PerformCommand();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PerformCommand()
        {
            // implement command here
        }


        public string Name { get { return COMMAND_NAME ; } }
        public string Caption { get { return "Parse Trace Results"; } }
        public string Tooltip { get { return "Select a query, the result will be fitted into a generated temporary table."; }}
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+D" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }

        public void Execute()
        {
            
        }

        public string MenuGroup
        {
            get { return "Data Generation"; }
        }

        public void Init(ISsmsFunctionalityProvider4 provider)
        {
            this.provider = provider;
            this.shellManager = new ShellManager(provider);
        }
    }
}