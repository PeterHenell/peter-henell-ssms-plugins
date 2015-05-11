using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    class DbTypeConverter
    {
        /// <summary>
        /// Translate .Net type to string value containing a corresponding SQL Server type.
        /// Note: Some datatypes will have precision defined that is not exactly same as it was in source tables.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string TranslateToSqlType(System.Type type)
        {
            var dbt = GetDBType(type);
            switch (dbt)
            {
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                    return dbt.ToString() + "(max)";
                case SqlDbType.DateTime:
                    return SqlDbType.DateTime2.ToString();
                case SqlDbType.Decimal:
                    return dbt.ToString() + "(30,6)";
                case SqlDbType.VarBinary:
                    return dbt.ToString() + "(max)";
                default:
                    return dbt.ToString();
            }

        }

        /// <summary>
        /// Stolen with pride from http://www.codeproject.com/Articles/16706/Convert-System-Type-to-SqlDbType
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        private static SqlDbType GetDBType(System.Type theType)
        {
            if (theType == typeof(byte[]))
            {
                return SqlDbType.VarBinary;
            }
            
            var p1 = new System.Data.SqlClient.SqlParameter();
            var tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType);
            if (tc.CanConvertFrom(theType))
            {
                p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
            }
            else
            {
                //Try brute force
                try
                {
                    p1.DbType = (DbType)tc.ConvertFrom(theType.Name);
                }
                catch (Exception)
                {
                    //Do Nothing
                }
            }
            return p1.SqlDbType;
        }
    }
}
