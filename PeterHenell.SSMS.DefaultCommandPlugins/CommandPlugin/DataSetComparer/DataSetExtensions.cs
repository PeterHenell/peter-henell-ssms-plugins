using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin.DataSetComparer
{
    public static class DataRowExtensions
    {
        public static List<DataRow> ToList(this DataRowCollection rows)
        {
            var list = new List<DataRow>();
            foreach (DataRow row in rows)
            {
                list.Add(row);
            }
            return list;
        }
    }

    public static class DataColumnExtensions
    {
        public static List<DataColumn> ToList(this DataColumnCollection columns)
        {
            var list = new List<DataColumn>();
            foreach (DataColumn col in columns)
            {
                list.Add(col);
            }
            return list;
        }
    }
}
