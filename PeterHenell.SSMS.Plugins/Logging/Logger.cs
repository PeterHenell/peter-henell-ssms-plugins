using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.Logging
{
    public static class Logger
    {
        public static void Info(string message)
        {
            Message("INFO", message);
        }

        private static void Message(string prefix, string message)
        {
            message = message == null ? string.Empty : message;
            //using (var pipe = new NamedPipeClientStream(".", "PipesOfPiece", PipeDirection.Out))
            //using (var stream = new StreamWriter(pipe))
            //{
            //    pipe.Connect();
            //    stream.Write(prefix + " [" + message + "]");
            //}
            Console.Write(prefix + " [" + message + "]");
        }

        public static void Error(string message)
        {
            Message("ERROR  ", message);
        }
    }
}
