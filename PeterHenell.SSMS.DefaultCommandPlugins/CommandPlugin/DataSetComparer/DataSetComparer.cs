//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin.DataSetComparer
//{
//    public class DataSetComparer
//    {

//        /// <summary>
//        /// Compares the contents of two sets with all their rows. Can be limited to compare only a 
//        ///  selected few columns.
//        /// </summary>
//        /// <param name="x">the first set</param>
//        /// <param name="y">the second set</param>
//        /// <param name="columnsToInclude">array of column names that should be part of the comparison. Default is all columns</param>
//        /// <returns>true if all rows in the first set exists and are equal in the second set</returns>
//        public bool SetsAreEqual(DataTable x, DataTable y, params string[] columnsToInclude)
//        {
//            if (x == null || y == null)
//            {
//                return false;
//            }

//            if (object.ReferenceEquals(x, y))
//                return true;

//            x = PrepareDataTable(x, columnsToInclude);
//            y = PrepareDataTable(y, columnsToInclude);

//            if (!(InternalCompare(x, y) && InternalCompare(y, x)))
//            {
//                //PrintTableDiffs(x, y);
//                return false;
//            }

//            return true;
//        }


//        private DataTable PrepareDataTable(DataTable table, string[] columnsToInclude)
//        {
//            if (columnsToInclude == null || columnsToInclude.Length == 0)
//            {
//                return table;
//            }
//            var clone = GetCloneWithFilteredColumns(table, columnsToInclude);
//            return clone;
//        }

//        private static DataTable GetCloneWithFilteredColumns(DataTable table, string[] columnsToInclude)
//        {
//            var clone = table.Copy();
//            var columnsToRemove = clone.Columns.ToList().Where(c => !columnsToInclude.Contains(c.ColumnName));
//            foreach (var col in columnsToRemove)
//            {
//                clone.Columns.Remove(col);
//            }
//            return clone;
//        }

//        //private void PrintTableDiffs(DataTable x, DataTable y)
//        //{
//        //    _writeLogline("Sets differ");
//        //    _writeLogline("");
//        //    _writeLogline("Set X:");
//        //    PrettyPrint(x);
//        //    _writeLogline("Set Y:");
//        //    PrettyPrint(y);
//        //}

//        private bool InternalCompare(DataTable x, DataTable y)
//        {
//            if (CompareColumns(x, y) == false)
//                return false;

//            if (CompareRows(x, y) == false)
//                return false;

//            return true;
//        }

//        private bool CompareRows(DataTable x, DataTable y)
//        {
//            if (x.Rows.Count != y.Rows.Count)
//                return false;

//            // http://powercollections.codeplex.com/
//            Wintellect.PowerCollections.Bag<DataRow> first = new Wintellect.PowerCollections.Bag<DataRow>(x.AsEnumerable(), DataRowComparer.Default);
//            Wintellect.PowerCollections.Bag<DataRow> second = new Wintellect.PowerCollections.Bag<DataRow>(y.AsEnumerable(), DataRowComparer.Default);

//            var diff = first.Difference(second);
//            if (diff.Count > 0)
//            {
//                return false;
//            }

//            return true;
//        }

//        private string GetColumnStringFormat(DataTable table)
//        {
//            var sb = new StringBuilder();

//            int colCounter = 0;
//            foreach (DataColumn col in table.Columns)
//            {
//                var maxWidth = table.Rows.ToList().Max(c => c[col].ToString().Length);
//                var formatLength = Math.Max(col.ColumnName.Length, maxWidth) + 15;
//                var formatString = "{" + (colCounter++) + ", -" + formatLength + " } ";
//                sb.Append(formatString);
//            }
//            return sb.ToString();
//        }

//        private bool CompareColumns(DataTable x, DataTable y)
//        {
//            // Makes sence since column names cannot be duplicated in a DataTable.
//            var orderedY = y.Columns.ToList().OrderBy(c => c.ColumnName);
//            var orderedX = x.Columns.ToList().OrderBy(c => c.ColumnName);

//            return orderedX.SequenceEqual(orderedY, DataColumnComparer.Default);
//        }
//        public class DataColumnComparer : IEqualityComparer<DataColumn>
//        {
//            static DataColumnComparer _comparer;
//            public static DataColumnComparer Default
//            {
//                get
//                {
//                    if (_comparer == null)
//                        _comparer = new DataColumnComparer();
//                    return _comparer;
//                }
//            }

//            public bool Equals(DataColumn x, DataColumn y)
//            {
//                return x.ColumnName == y.ColumnName;
//            }

//            public int GetHashCode(DataColumn obj)
//            {
//                return obj.ColumnName.GetHashCode();
//            }
//        }
//    }
//}
