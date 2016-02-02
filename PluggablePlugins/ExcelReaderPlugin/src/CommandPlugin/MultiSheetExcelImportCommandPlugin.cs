using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Plugins;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderTest.CommandPlugin
{
    public class MultiSheetExcelImportCommandPlugin : CommandPluginBase
    {
        public MultiSheetExcelImportCommandPlugin()
            : base("MultiSheetExcelImportCommand", CommandPluginBase.MenuGroups.DataGeneration, "Import Multiple Excel Sheets into tables", "global::ctlr+alt+i")
        {

        }

        public override void ExecuteCommand()
        {
            var file = DialogManager.ShowSelectFileDialog();
            if (file == null)
                return;

            var ddlManager = new DDLManager();
            var bulkdInsertManager = new BulkInsertManager();
            String connString = ConnectionManager.GetConnectionStringForCurrentWindow();
            Console.WriteLine("Start time" + DateTime.Now);

            ExcelStreamReader.Execute(file.FullName, reader =>

                reader.ForEachSheet(sheet =>
                {
                    var schema = sheet.GetSchema();

                    ddlManager.CreateTable(schema.TableName, schema, connString);
                    bulkdInsertManager.BulkInsertTo(schema, schema.TableName, sheet, connString);
                })
            );
        }
    }
}
