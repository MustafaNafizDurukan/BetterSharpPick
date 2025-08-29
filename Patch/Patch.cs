using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BetterSharpPick.Patch
{
    public static class Patcher
    {
        public static void Patch()
        {
            ETW.Patch();
            AMSI.Patch();
        }
    }
}
