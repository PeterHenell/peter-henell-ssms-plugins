using RedGate.SIPFrameworkShared;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System;
using EnvDTE80;
using EnvDTE;
using Microsoft.SqlServer.Management.UI.VSIntegration;
//using Microsoft.SqlServer.Management.UI.ConnectionDlg;
using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;

namespace SampleSsmsEcosystemAddin
{
    public class SharedCommand : ISharedCommandWithExecuteParameter
    {
        private readonly ISsmsFunctionalityProvider4 m_Provider;
        private readonly ICommandImage m_CommandImage = new CommandImageNone();

        public SharedCommand(ISsmsFunctionalityProvider4 provider)
        {
            m_Provider = provider;           
        }

        public string Name { get { return "GenerateTempTablesFromSelectedQuery_Command"; } }
        public void Execute(object parameter)
        {
            var editPoint = GetEditPointAtBottomOfSelection();
            var currentWindow = m_Provider.GetQueryWindowManager();
            var contents = currentWindow.GetActiveAugmentedQueryWindowContents();
            
            string tempTableDefinitions = CreateTempTablesFromQueryResult(contents);
            editPoint.Insert("\n" + tempTableDefinitions);
        }

        private EditPoint GetEditPointAtBottomOfSelection()
        {
            DTE2 a = (DTE2)m_Provider.SsmsDte2;
            Document document = a.ActiveDocument;
            if (document != null)
            {
                // find the selected text, and return the edit point at the bottom of it.
                TextSelection selection = document.Selection as TextSelection;
                return selection.BottomPoint.CreateEditPoint();
            }
            return null;
        }

        private string GetConnectionString(UIConnectionInfo connection)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = connection.ServerName;
            builder.IntegratedSecurity = string.IsNullOrEmpty(connection.Password);

            builder.Password = connection.Password;
            builder.UserID = connection.UserName;
            builder.InitialCatalog = connection.AdvancedOptions["DATABASE"] ?? "master";
            builder.ApplicationName = "Peter Henell Plugins";

            return builder.ToString();
        }

        private string CreateTempTablesFromQueryResult(string input)
        {
            var sb = new StringBuilder();

            try
            {
                IScriptFactory scriptFactory = ServiceCache.ScriptFactory;
                string connectionString = "";

                if (scriptFactory == null)
                {
                    return "ServiceCache.ScriptFactory is null";
                }
                else
                {
                    connectionString = GetConnectionString(scriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo);
                }

                var cmd = new SqlCommand(string.Format("SET FMTONLY ON; {0}", input), new SqlConnection(connectionString));
                var ad = new SqlDataAdapter(cmd);
                var ds = new DataSet();

                ad.Fill(ds);
                int tableCounter = 1;
                foreach (DataTable metaTable in ds.Tables)
                {
                    string tempTable = string.Format("#temp{0}", tableCounter);
                    sb.AppendFormat("IF OBJECT_ID('temp..{0}') IS NOT NULL DROP TABLE {0};", tempTable);

                    sb.AppendLine();
                    sb.AppendFormat("CREATE TABLE {0} (", tempTable);
                    sb.AppendLine();

                    int columnCount = 1;
                    foreach (DataColumn col in metaTable.Columns)
                    {
                        sb.AppendFormat("\t [{0}] {1}", col.ColumnName, TranslateToSqlType(col.DataType).ToUpper());

                        if (columnCount < metaTable.Columns.Count)
                            sb.Append(",");

                        sb.AppendLine();
                        columnCount++;
                    }

                    sb.Append(");");
                    sb.AppendLine();
                    sb.AppendLine();
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(" ... ");
                sb.AppendFormat("An error occured during generation: [{0}]", ex.ToString());
            }

            return sb.ToString();
        }

        private string TranslateToSqlType(System.Type type)
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
                    return dbt.ToString() + "(19,6)";
                default:
                    return dbt.ToString();
            }
            
        }
        /// <summary>
        /// Stolen with pride from http://www.codeproject.com/Articles/16706/Convert-System-Type-to-SqlDbType
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        private SqlDbType GetDBType(System.Type theType)
        {
            System.Data.SqlClient.SqlParameter p1 = null;
            System.ComponentModel.TypeConverter tc = null;
            p1 = new System.Data.SqlClient.SqlParameter();
            tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType);
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

        public string Caption { get { return "Generate Temp Tables From Selected Queries"; } }
        public string Tooltip { get { return "Select a query, the result will be fitted into a generated temporary table."; }}
        public ICommandImage Icon { get { return m_CommandImage; } }
        public string[] DefaultBindings { get { return new[] { "global::Ctrl+Alt+D" }; } }
        public bool Visible { get { return true; } }
        public bool Enabled { get { return true; } }


        public void Execute()
        {
            
        }
    }
}