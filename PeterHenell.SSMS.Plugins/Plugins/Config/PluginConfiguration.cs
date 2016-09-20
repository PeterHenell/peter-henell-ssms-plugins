using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Plugins.Config
{
    [Serializable]
    public class PluginConfiguration : Dictionary<string, string>
    {
        public PluginConfiguration(string ownerName)
        {
            this.OwnerName = ownerName;
        }

        private PluginConfiguration()
        {

        }

        public PluginConfiguration(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

        public string OwnerName { get; set; }
    }
}
