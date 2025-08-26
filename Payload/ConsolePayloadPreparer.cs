using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using BetterSharpPick.Parsing;

namespace BetterSharpPick.Payload
{
    public class ConsolePayloadPreparer : IPayloadPreparer
    {
        public string Prepare(Options opts)
        {
            if (opts.CommandArgs == null) opts.CommandArgs = new string[0];

            byte[] dataBytes = null;
            string source = null;

            if (!string.IsNullOrWhiteSpace(opts.Command))
            {
                dataBytes = Encoding.UTF8.GetBytes(opts.Command);
                source = "inline (-c)";
            }
            else if (!string.IsNullOrWhiteSpace(opts.PathOrUrl))
            {
                dataBytes = LoadBytes(opts.PathOrUrl);
                source = opts.PathOrUrl;
            }
            else
                throw new ArgumentException("No input to process: provide either '-c <text>' or '-path <file|url>'.");

            if (opts.UseBase64)
                dataBytes = DecodeBase64ToBytes(dataBytes);

            if (opts.UseXor)
                dataBytes = ApplyXor(dataBytes, opts.XorKey);

            string decodedText = BytesToUtf8(dataBytes);

            string[] decodedArgs = opts.CommandArgs ?? Array.Empty<string>();

#if DEBUG
            Console.WriteLine("    source        : " + source);
            Console.WriteLine("    payload       : " + Preview(decodedText, 200));
            Console.WriteLine("    args          : [" + string.Join(", ", decodedArgs) + "]");
            Console.WriteLine("    xor?          : " + opts.UseXor);
            Console.WriteLine("    xorKey        : " + opts.XorKey);
#endif
            return string.Join(Environment.NewLine, new[] { decodedText }.Concat(decodedArgs ?? Array.Empty<string>()));
        }

        private static byte[] DecodeBase64ToBytes(byte[] data)
        {
            if (data == null || data.Length == 0) return Array.Empty<byte>();

            var asText = Encoding.ASCII.GetString(data);
            asText = StripWhitespace(asText);

            try
            {
                return Convert.FromBase64String(asText);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid Base64 payload content.", ex);
            }
        }

        private static string StripWhitespace(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if (!char.IsWhiteSpace(ch)) sb.Append(ch);
            }
            return sb.ToString();
        }

        private static byte[] LoadBytes(string pathOrUrl)
        {
            if (File.Exists(pathOrUrl))
                return File.ReadAllBytes(pathOrUrl);

            Uri uri;
            if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(15);
                    return http.GetByteArrayAsync(uri).GetAwaiter().GetResult();
                }
            }

            throw new ArgumentException("Invalid path/URL: " + pathOrUrl);
        }

        private static byte[] ApplyXor(byte[] data, byte key)
        {
            if (data == null || data.Length == 0) return new byte[0];
            var output = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                output[i] = (byte)(data[i] ^ key);
            return output;
        }

        private static string XorString(string value, byte key)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(value);
            var outBytes = ApplyXor(bytes, key);
            return BytesToUtf8(outBytes);
        }

        private static string BytesToUtf8(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return string.Empty;
            return Encoding.UTF8.GetString(bytes);
        }

        private static string Preview(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            s = s.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
            return s.Length > max ? s.Substring(0, max) + "..." : s;
        }
    }
}
