using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Forms;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using RedGate.SIPFrameworkShared;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using PeterHenell.SSMS.Plugins.Plugins;
using System.IO;
using Excel;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ImportExcelCommand : ICommandPlugin
    {
        public readonly static string COMMAND_NAME = "ImportExcelCommand";

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
                MessageBox.Show(ex.ToString());
            }
        }

        private void PerformCommand()
        {
            // select file
            // choose name of target table
            // create table
            // stream excel file into table

            var file = DialogManager.ShowSelectFileDialog();
            if (file == null)
                return;

            //var tableName = "tempImport";

            var ok = new Action<string>( (tableName) =>
            {
                ExcelImporter importer = new ExcelImporter();
                DDLManager ddlManager = new DDLManager();

                var meta = importer.GetMetaData(file.FullName);
                ddlManager.CreateTable(tableName, meta);
                importer.ImportToTable(file.FullName, tableName, meta);

                shellManager.AppendToEndOfSelection("SELECT * FROM " + tableName);
                shellManager.AppendToEndOfSelection("--DROP TABLE " + tableName);
            });


            DialogManager.GetDialogInputFromUser("Choose target table name", "ExcelImport", ok);
        }

        

        private void cancelCallback()
        {
        }

        public string Name { get { return COMMAND_NAME; } }
        public string Caption { get { return "Import Excel File (Only reads Sheet 1)"; } }
        public string Tooltip { get { return "Import Excel File (Only reads Sheet 1)"; } }
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+E" }; } }
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