using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Logging
{
    public class PipeTextWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        public override void WriteLine(string value)
        {
            using (var pipe = new NamedPipeClientStream(".", "PipesOfPiece", PipeDirection.Out))
            using (var stream = new StreamWriter(pipe))
            {
                pipe.Connect();
                stream.WriteLineAsync(value);
            }
            //Console.WriteLine(value);
        }
    }
}