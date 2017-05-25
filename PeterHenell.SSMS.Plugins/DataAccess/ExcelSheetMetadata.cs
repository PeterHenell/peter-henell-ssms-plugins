using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    public class ExcelSheetMetadata
    {
        public class ExcelColumnMetadata
        {
            public string ColumnName { get; set; }
        }

        public ExcelSheetMetadata()
        {
            this.Columns = new List<ExcelColumnMetadata>();
        }

        internal void AddColumn(String columnName)
        {
            var val = new ExcelColumnMetadata { ColumnName = columnName };
            Columns.Add(val);
        }

        public List<ExcelColumnMetadata> Columns { get; private set; }
    }
}
