using System;
using System.Diagnostics;

using BordeX.Native;

namespace BordeX
{
    public static class ProcessUtils
    {
        public static bool IsInWin64Emulator(Process process)
        {
            bool retVal;
            return WinAPI.IsWow64Process(process.Handle, out retVal) && retVal;
        }
    }
}
