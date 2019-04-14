using System;
using System.IO;
using System.Diagnostics;

using Microsoft.Win32;

namespace BordeX
{
    public static class RegistryUtils
    {
        private static string Key_Auto_Startup = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        public static void SetAutoStartup(bool enabled)
        {
            Logger.LogInfo("Modifying Startup Registry: " + enabled);
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Key_Auto_Startup, true);
            if (enabled) key.SetValue(References.Name, Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), RegistryValueKind.String);
            else key.DeleteValue(References.Name, false);
        }
    }
}
