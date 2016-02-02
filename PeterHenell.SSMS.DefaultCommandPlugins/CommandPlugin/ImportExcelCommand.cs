using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using PeterHenell.SSMS.Plugins.Plugins;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class ImportExcelCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "ImportExcelCommand";

        public ImportExcelCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.DataGeneration,
                 "Import Excel File (Only reads Sheet 1)",
                 "global::Ctrl+Alt+E")
        {

        }

        public override void ExecuteCommand()
        {
            var file = DialogManager.ShowSelectFileDialog();
            if (file == null)
                return;

            var ok = new Action<string>( (tableName) =>
            {
                ExcelImporter importer = new ExcelImporter();
                DDLManager ddlManager = new DDLManager();

                var meta = importer.GetMetaData(file.FullName);
                ddlManager.CreateTable(tableName, meta);
                importer.ImportToTable(file.FullName, tableName, meta);

                ShellManager.AppendToEndOfSelection("SELECT * FROM " + tableName);
                ShellManager.AppendToEndOfSelection("--DROP TABLE " + tableName);
            });


            DialogManager.GetDialogInputFromUser("Choose target table name (This table will be created)", "ExcelImport", ok);
        }

        

        private void cancelCallback()
        {
        }

        //public string Name { get { return COMMAND_NAME; } }
        //public string Caption { get { return "Import Excel File (Only reads Sheet 1)"; } }
        //public string Tooltip { get { return "Import Excel File (Only reads Sheet 1)"; } }
        //public ICommandImage Icon { get { return m_CommandImage; } }
        //public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+E" }; } }
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