using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Forms;
using PeterHenell.SSMS.Plugins.Shell;
using PeterHenell.SSMS.Plugins.Utils;
using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using PeterHenell.SSMS.Plugins.ExtensionMethods;
using PeterHenell.SSMS.Plugins.Plugins;
using System.Threading;
using PeterHenell.SSMS.Plugins.DataAccess.DTO;

namespace PeterHenell.SSMS.Plugins.Commands
{
    public class OpenFileOnDiskCommand : CommandPluginBase
    {
        public readonly static string COMMAND_NAME = "OpenFileOnDiskCommand";

        public OpenFileOnDiskCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.Liquibase,
                 "Open selected object on disk",
                 "global::F12")
        {

        }

        public override void ExecuteCommand(CancellationToken token)
        {
            
        }

    }
}