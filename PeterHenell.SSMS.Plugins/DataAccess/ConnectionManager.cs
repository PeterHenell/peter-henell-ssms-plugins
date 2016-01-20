using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.Editors;
using PeterHenell.SSMS.Plugins.Utils;
using RedGate.SIPFrameworkShared;
using System;
using System.Data.SqlClient;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    class ConnectionManager
    {
        internal static string GetConnectionString(UIConnectionInfo connection, string defaultDatabase = "master")
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = connection.ServerName;
            builder.IntegratedSecurity = string.IsNullOrEmpty(connection.Password);

            builder.Password = connection.Password;
            builder.UserID = connection.UserName;
            builder.InitialCatalog = connection.AdvancedOptions["DATABASE"] ?? defaultDatabase;
            builder.ApplicationName = "Peter Henell Plugins";

            return builder.ToString();
        }

        internal static string GetConnectionStringForCurrentNode()
        {
            var objectExplorerNode = NodeManager.CurrentNode as IOeNode;

            IConnectionInfo ci = null;
            if (objectExplorerNode != null
                    && objectExplorerNode.HasConnection
                    && objectExplorerNode.TryGetConnection(out ci))
            {
                var builder = new SqlConnectionStringBuilder(ci.ConnectionString);
                return builder.ConnectionString;
            }
            throw new InvalidOperationException("No selected db node or other problem...");
        }

        internal static string GetConnectionStringForCurrentWindow()
        {
            IScriptFactory scriptFactory = ServiceCache.ScriptFactory;
            if (scriptFactory != null)
                return GetConnectionString(scriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo);
            else
                throw new InvalidOperationException("ServiceCache.ScriptFactory is null. This usually happens when wrong assemblies have been referenced.");                
        }
    }
}
