using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Logging
{
    public class StringBuilderTextWriter : TextWriter
    {
        public StringBuilderTextWriter(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }
            this._sb = sb;
        }

        public override void WriteLine(string value)
        {
            _sb.AppendLine(value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        private StringBuilder _sb;
    }
}
