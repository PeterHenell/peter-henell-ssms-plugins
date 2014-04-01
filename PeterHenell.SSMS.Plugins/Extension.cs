using System;
using RedGate.SIPFrameworkShared;

namespace PeterHenell.SSMS.Plugins
{
    public class Extension : ISsmsAddin
    {
        private ISsmsFunctionalityProvider4 m_Provider4;
        private object m_Dte2;

        public void OnLoad(ISsmsExtendedFunctionalityProvider provider)
        {
            m_Provider4 = (ISsmsFunctionalityProvider4) provider;
            
            m_Dte2 = m_Provider4.SsmsDte2;

            if(m_Provider4 == null)
                throw new ArgumentException();

            var subMenus = new SimpleOeMenuItemBase[]
            {
                new Menu("Command 1", m_Provider4)
            };

            m_Provider4.AddGlobalCommand(new SharedCommand(m_Provider4));

            m_Provider4.MenuBar.MainMenu.BeginSubmenu("Peter Henell", "Peter Henell")
                .BeginSubmenu("Code Generation", "Code Generation")
                .AddCommand("GenerateTempTablesFromSelectedQuery_Command")
                .EndSubmenu();

            m_Provider4.AddTopLevelMenuItem(new Submenu(subMenus));
        }

        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
            //Called when object explorer node selection changes.
            
        }

        public string Version { get { return "Peter Henell SSMS Plugins 2014 1.0"; } }
    }
}