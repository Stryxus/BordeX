using System;
using System.Runtime.InteropServices;

namespace BordeX.Native
{
    public static class Steam
    {
        [DllImport("BordeX.Native.dll", EntryPoint = "InitializeSteam", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Initialize(long processID);

        [DllImport("BordeX.Native.dll", EntryPoint = "ShutdownSteam", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Shutdown();
    }
}
