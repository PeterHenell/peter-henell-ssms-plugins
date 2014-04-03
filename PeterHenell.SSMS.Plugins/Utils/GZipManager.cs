using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace PeterHenell.SSMS.Plugins.Utils
{
    public class GZipManager
    {
        public static string DecompressBase64EncodedString(string base64EncodedString)
        {
            var bytes = System.Convert.FromBase64String(base64EncodedString);
            var unzipped = Decompress(bytes);
            return unzipped;
        }

        public static string Decompress(byte[] byteArray)
        {
            StringBuilder uncompressed = new StringBuilder();

            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[1024];

                int readBytes;
                while ((readBytes = gZipStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    for (int i = 0; i < readBytes; i++)
                        uncompressed.Append((char)buffer[i]);
                }
            }

            return uncompressed.ToString();

        }

        public static byte[] Compress(string text, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text cannot be null nor empty");

            encoding = encoding ?? System.Text.Encoding.Unicode;
            byte[] inputBytes = encoding.GetBytes(text);
            using (MemoryStream resultStream = new MemoryStream())
            using (GZipStream gZipStream = new GZipStream(resultStream, CompressionMode.Compress))
            {
                gZipStream.Write(inputBytes, 0, inputBytes.Length);
                return resultStream.ToArray();
            }
        }
    }
}
