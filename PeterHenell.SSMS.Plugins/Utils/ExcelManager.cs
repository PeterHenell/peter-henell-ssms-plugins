using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class ExcelManager
    {

        internal static void TableToExcel(System.Data.DataSet ds, FileInfo file)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                int counter = 0;
                foreach (DataTable table in ds.Tables)
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet " + counter++);
                    ws.Cells["A1"].LoadFromDataTable(table, true);
                }

                pck.SaveAs(file);
            }
        }
    }
}
