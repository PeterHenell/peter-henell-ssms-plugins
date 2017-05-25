using FizzWare.NBuilder;
using PeterHenell.SSMS.Plugins.TypeBuilders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.Utils.Generators
{
    public class DataGenerator
    {
        public void Fill(DataTable schema, int rows)
        {
            var builder = new DynamicTypeBuilder(schema);

            var o = builder.CreateNewObject(schema);
            Type t = o.GetType();
            var entities = GetListOfGeneratedObjects(o, rows);
            foreach (var entity in entities)
            {
                var row = schema.NewRow();
                foreach (var prop in t.GetProperties())
                {
                    row[prop.Name] = t.GetProperty(prop.Name).GetValue(entity);
                }
                schema.Rows.Add(row);
            }
        }

        public static IList<T> GetListOfGeneratedObjects<T>(T objectTemplate, int rows)
        {
            return Builder<T>.CreateListOfSize(rows).Build();
        }
    }
}
