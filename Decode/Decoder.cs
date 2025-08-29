using System;
using System.Text;

namespace BetterSharpPick.Services
{
    public class Decoder
    {
        public bool UseBase64 { get; set; }

        public Decoder()
        {
        }

        public string Decode(string value)
        {
            if (!UseBase64) return value ?? string.Empty;
            if (string.IsNullOrEmpty(value)) return string.Empty;

            try
            {
                byte[] raw = Convert.FromBase64String(value);
                return Encoding.UTF8.GetString(raw);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid Base64 value: '" + value + "'");
            }
        }
    }
}
