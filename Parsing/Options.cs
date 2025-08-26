using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSharpPick.Parsing
{
    public class Options
    {
        public bool UseBase64 { get; set; }
        public bool UseXor { get; set; }
        public byte XorKey { get; set; }

        public string PathOrUrl { get; set; }
        public string Command { get; set; }
        public string[] CommandArgs { get; set; }

        public Options()
        {
            XorKey = 0;
            PathOrUrl = string.Empty;
            Command = string.Empty;
            CommandArgs = new string[0];
        }
    }
}
