using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.DataAccess.DTO
{
    public class ObjectReference
    {
        public string EntityName { get; set; }

        public ObjectMetadata ReferencedObject { get; set; }
    }
}
