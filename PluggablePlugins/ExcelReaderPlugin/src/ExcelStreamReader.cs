using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReaderTest
{
    public class ExcelStreamReader : IDisposable
    {
        private string fileName;
        private SpreadsheetDocument doc;
        private WorkbookPart workbookPart;
        private List<Sheet> sheets;


        private ExcelStreamReader(string fileName)
        {
            this.fileName = fileName;
            this.doc = SpreadsheetDocument.Open(fileName, false);
            this.workbookPart = doc.WorkbookPart;
            this.sheets = GetSheets();
        }

        public static void Execute(string fileName, Action<ExcelStreamReader> action)
        {
            var reader = new ExcelStreamReader(fileName);
            action(reader);
        }

        private List<Sheet> GetSheets()
        {
            var sheets = workbookPart.Workbook.Sheets.Cast<Sheet>().ToList();
            return sheets;
        }

        private Sheet GetCurrentSheet(WorksheetPart worksheetPart)
        {
            var sheetId = workbookPart.GetIdOfPart(worksheetPart);
            var correspondingSheet = sheets.FirstOrDefault(
                s => s.Id.HasValue && s.Id.Value == sheetId);
            return correspondingSheet;
        }

        private void PrintSheetNames(List<Sheet> sheets)
        {
            sheets.ForEach(x => Console.WriteLine(
                  String.Format("RelationshipId:{0}\n SheetName:{1}\n SheetId:{2}"
                  , x.Id.Value, x.Name.Value, x.SheetId.Value)));
        }

        public void ForEachSheet(Action<SheetOperator> action)
        {
            using (var sharedStringCache = new SharedStringCache(workbookPart))
            {
                foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
                {
                    var currentSheet = GetCurrentSheet(worksheetPart);

                    using (OpenXmlReader reader = OpenXmlReader.Create(worksheetPart))
                    using (SheetOperator sheetOperator = new SheetOperator(workbookPart, reader, currentSheet, sharedStringCache))
                    {
                        action(sheetOperator);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (doc != null)
            {
                doc.Dispose();
            }
        }
    }
}
