using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Logging
{
    public class DebugTextWriter : TextWriter
    {
        public override void WriteLine(string value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}
