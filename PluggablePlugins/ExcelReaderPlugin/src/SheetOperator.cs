using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace ExcelReaderTest
{
    public class SheetOperator : IDisposable, IDataReader
    {
        private WorkbookPart _workbookPart;
        private OpenXmlReader _reader;
        private Sheet _currentSheet;
        private DataTable _schema;
        private DataRow _currentRow;
        private SharedStringCache _sharedStringCache;

        public SheetOperator(WorkbookPart workbookPart, OpenXmlReader reader, Sheet currentSheet, SharedStringCache sharedStringCache)
        {
            this._workbookPart = workbookPart;
            this._reader = reader;
            this._currentSheet = currentSheet;
            this._sharedStringCache = sharedStringCache;
        }

        public void ForEachRow(Action<DataTable, DataRow> forEachRow)
        {
            while (_reader.Read())
            {
                if (_reader.ElementType == typeof(Row))
                {
                    if (_schema != null)
                    {
                        DataRow dataRow = ProcessOneRow();
                        forEachRow(_schema, dataRow);
                    }
                    else
                    {
                        CreateSchema();
                    }
                }
            }
        }

        public IEnumerable<DataRow> GetRows()
        {
            while (_reader.Read())
            {
                if (_reader.ElementType == typeof(Row))
                {
                    if (_schema != null)
                    {
                        DataRow dataRow = ProcessOneRow();
                        yield return dataRow;
                    }
                    else
                    {
                        CreateSchema();
                    }
                }
            }
        }

        public IDataReader GetDataReader()
        {
            return this;
        }

        public DataTable GetSchema()
        {
            while (_reader.Read())
            {
                if (_reader.ElementType == typeof(Row))
                {
                    CreateSchema();
                    return _schema;
                }
            }
            throw new InvalidOperationException("Could not find any rows in sheet:" + _currentSheet.Name);
        }

        private DataRow ProcessOneRow()
        {
            var dataRow = _schema.NewRow();
            var colCount = 0;

            _reader.ReadFirstChild();
            do
            {
                if (_reader.ElementType == typeof(Cell))
                {
                    Cell c = (Cell)_reader.LoadCurrentElement();
                    string cellValue = GetCellValue(c, "");

                    dataRow[colCount++] = cellValue;
                }

            } while (_reader.ReadNextSibling());

            return dataRow;
        }

        private string GetCellValue(Cell c, string defaultValue)
        {
            string cellValue;
            if (c.DataType != null && c.DataType == CellValues.SharedString)
            {
                var shindex = int.Parse(c.CellValue.InnerText);
                SharedStringItem ssi = _sharedStringCache.Get(shindex);
                cellValue = ssi.Text.Text;
            }
            else
            {
                cellValue = c.CellValue != null ? c.CellValue.InnerText : defaultValue;
            }
            return cellValue;
        }

        /// <summary>
        /// Creates schema from first row(or any row, this method does not care)
        /// </summary>
        /// <returns></returns>
        private void CreateSchema()
        {
            if (_schema != null)
                return;

            _schema = new DataTable(_currentSheet.Name);
            _reader.ReadFirstChild();
            var colCount = 0;
            do
            {
                if (_reader.ElementType == typeof(Cell))
                {
                    Cell c = (Cell)_reader.LoadCurrentElement();
                    string cellValue = GetCellValue(c, "Column" + colCount);
                    
                    _schema.Columns.Add(cellValue, typeof(string));
                    colCount++;
                }

            } while (_reader.ReadNextSibling());
        }

        public void Dispose()
        {
            if (_schema != null)
            {
                _schema.Dispose();
            }
        }

        public void Close()
        {

        }

        private bool MoveNext()
        {
            while (_reader.Read())
            {
                if (_reader.ElementType == typeof(Row))
                {
                    DataRow dataRow = ProcessOneRow();
                    _currentRow = dataRow;
                    return true;
                }
            }
            return false;
        }

        public int Depth
        {
            get { return 1000000; }
        }

        public DataTable GetSchemaTable()
        {
            return _schema;
        }

        public bool IsClosed
        {
            get { return _schema == null; }
        }

        public bool NextResult()
        {
            return MoveNext();
        }

        public bool Read()
        {
            return MoveNext();
        }

        public int RecordsAffected
        {
            get { return 1; }
        }

        public int FieldCount
        {
            get { return _schema.Columns.Count; }
        }

        public bool GetBoolean(int ordinal)
        {
            return GetFieldValue<bool>(ordinal);
        }

        public byte GetByte(int ordinal)
        {
            return GetFieldValue<byte>(ordinal);
        }

        public long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int ordinal)
        {
            return GetFieldValue<char>(ordinal);
        }

        public long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int ordinal)
        {
            return _schema.Columns[ordinal].DataType.Name;
        }

        public DateTime GetDateTime(int ordinal)
        {
            return GetFieldValue<DateTime>(ordinal);
        }

        public decimal GetDecimal(int ordinal)
        {
            return GetFieldValue<Decimal>(ordinal);
        }

        public double GetDouble(int ordinal)
        {
            return GetFieldValue<double>(ordinal);
        }

        public Type GetFieldType(int ordinal)
        {
            return _schema.Columns[ordinal].DataType;
        }

        public float GetFloat(int ordinal)
        {
            return GetFieldValue<float>(ordinal);
        }

        public Guid GetGuid(int ordinal)
        {
            return GetFieldValue<Guid>(ordinal);
        }

        public short GetInt16(int ordinal)
        {
            return GetFieldValue<short>(ordinal);
        }

        public int GetInt32(int ordinal)
        {
            return GetFieldValue<int>(ordinal);
        }

        public long GetInt64(int ordinal)
        {
            return GetFieldValue<long>(ordinal);
        }

        public string GetName(int ordinal)
        {
            return _schema.Columns[ordinal].ColumnName;
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < _schema.Columns.Count; i++)
            {
                if (_schema.Columns[i].ColumnName == name)
                {
                    return i;
                }
            }
            throw new ArgumentOutOfRangeException("Column not found in schema");
        }

        public string GetString(int ordinal)
        {
            return GetFieldValue<string>(ordinal);
        }

        public object GetValue(int ordinal)
        {
            return _currentRow[ordinal];
        }

        public int GetValues(object[] values)
        {
            values = _currentRow.ItemArray;
            return values.Length;
        }

        public bool HasRows
        {
            get { return _schema != null; }
        }

        public bool IsDBNull(int ordinal)
        {
            return _currentRow[ordinal] == null;
        }

        public object this[string name]
        {
            get
            {
                var ordinal = GetOrdinal(name);
                return GetFieldValue<object>(ordinal);
            }
        }

        public object this[int ordinal]
        {
            get { return _currentRow[ordinal]; }
        }

        public IDataReader GetData(int i)
        {
            return this;
        }

        private TColType GetFieldValue<TColType>(int ordinal)
        {
            if (_currentRow == null)
            {
                throw new InvalidOperationException("Cannot get value before calling Read()");
            }

            return (TColType)_currentRow[ordinal];
        }
    }
}
