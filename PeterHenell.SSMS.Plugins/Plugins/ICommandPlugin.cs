using RedGate.SIPFrameworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins
{
    public interface ICommandPlugin : ISharedCommandWithExecuteParameter
    {
        string CommandName { get; }
        string MenuGroup { get; }
        void Init(ISsmsFunctionalityProvider4 provider);
    }
}
