using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterHenell.SSMS.Plugins.ExtensionMethods;

namespace PeterHenell.SSMS.Plugins.QueryBuilders
{
    public class LambdaInsertBuilder
    {
        private readonly StringBuilder _queryBuilder;
        private ValuesBuilder valuesBuilder;

        public LambdaInsertBuilder()
        {
            this._queryBuilder = new StringBuilder();
            this.valuesBuilder = new ValuesBuilder(this);
        }

        public LambdaInsertBuilder Append(string text)
        {
            _queryBuilder.Append(text);
            return this;
        }

        public LambdaInsertBuilder AppendLine(string text)
        {
            _queryBuilder.AppendLine(text);
            return this;
        }

        public LambdaInsertBuilder Insert(string targetTable)
        {
            _queryBuilder.Append("INSERT INTO ");
            _queryBuilder.Append(targetTable);
            return this;
        }

        public ValuesBuilder Columns(DataTable schema, bool newLineBetweenColumns = false, string columnDelimiter = " ")
        {
            _queryBuilder.Append(" (");
            _queryBuilder.AppendColumnNameList(schema, newLineBetweenColumns, columnDelimiter);
            _queryBuilder.Append(")");
            _queryBuilder.AppendLine();
            return this.valuesBuilder;
        }

        public string End()
        {
            return _queryBuilder.ToString();
        }

        public static string Build(Func<LambdaInsertBuilder, string> func)
        {
            return func(new LambdaInsertBuilder());
        }


        public class ValuesBuilder
        {
            private LambdaInsertBuilder outerBuilder;

            public ValuesBuilder(LambdaInsertBuilder insertBuilder)
            {
                this.outerBuilder = insertBuilder;
            }
            public LambdaInsertBuilder Values(DataTable rows, bool newLineBetweenValues = false)
            {
                outerBuilder._queryBuilder.Append("VALUES ");
                outerBuilder._queryBuilder.AppendListOfRows(rows, newLineBetweenValues);
                return outerBuilder;
            }
            public LambdaInsertBuilder Query(string query)
            {
                outerBuilder._queryBuilder.Append(query);
                return outerBuilder;
            }
        }
    }
}
