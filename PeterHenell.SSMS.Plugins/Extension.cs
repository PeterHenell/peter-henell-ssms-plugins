using System;
using RedGate.SIPFrameworkShared;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Commands;
using System.Collections.Generic;
using PeterHenell.SSMS.Plugins.Utils;
using System.Windows.Forms;

namespace PeterHenell.SSMS.Plugins
{
    public class Extension : ISsmsAddin
    {
        private ISsmsFunctionalityProvider4 m_Provider4;
        private object m_Dte2;

        List<ISharedCommandWithExecuteParameter> commands = new List<ISharedCommandWithExecuteParameter>();

        public void OnLoad(ISsmsExtendedFunctionalityProvider provider)
        {
            m_Provider4 = (ISsmsFunctionalityProvider4)provider;

            m_Dte2 = m_Provider4.SsmsDte2;

            if (m_Provider4 == null)
                throw new ArgumentException();


            commands.Add(new TempTablesFromSelectionCommand(m_Provider4));
            commands.Add(new DecompressResultCommand(m_Provider4));
            commands.Add(new GenerateInsertStatementCommand(m_Provider4));
            commands.Add(new MockAndInsertCommand(m_Provider4));
            commands.Add(new ResultToExcelCommand(m_Provider4));
            commands.Add(new ActualAndExpectedCommand(m_Provider4));
            commands.Add(new GenerateDataForTableCommand(m_Provider4));

            // STEP 1: Add command to the provider
            foreach (var command in commands)
            {
                m_Provider4.AddGlobalCommand(command);
            }

            // STEP 2: Add command to menu
            m_Provider4.MenuBar.MainMenu.BeginSubmenu("Peter Henell", "Peter Henell")
                .BeginSubmenu("Code Generation", "Code Generation")
                .AddCommand(TempTablesFromSelectionCommand.COMMAND_NAME)
                .AddCommand(DecompressResultCommand.COMMAND_NAME)
                .AddCommand(GenerateInsertStatementCommand.COMMAND_NAME)
                .AddCommand(ResultToExcelCommand.COMMAND_NAME)
                .AddCommand(GenerateDataForTableCommand.COMMAND_NAME)
                .EndSubmenu()
                .BeginSubmenu("tSQLt - Tools", "tSQLt - Tools")
                .AddCommand(MockAndInsertCommand.COMMAND_NAME)
                .AddCommand(ActualAndExpectedCommand.COMMAND_NAME)
                .EndSubmenu()
                .EndSubmenu();
        }

        public void OnNodeChanged(ObjectExplorerNodeDescriptorBase node)
        {
            NodeManager.CurrentNode = node;
        }

        public string Version { get { return "Peter Henell SSMS Plugins 2015 1.1.0"; } }
        public string Author { get { return "Peter Henell"; } }
        public string URL { get { return "https://github.com/PeterHenell/peter-henell-ssms-plugins"; } }

    }
}