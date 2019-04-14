using System;
using System.Windows.Forms;

using BordeX.Native;
using BordeX.Windows;

namespace BordeX
{
    public static class Common
    {
        public static BaseWindow Window;

        public static Program ProgramInstance;
        public static NotifyIcon NotificationIcon;
        public static Taskbar TaskbarInstance;

        // Store the OS so that we can keep the app from running old hardware and untested OS's
        public static OperatingSystem OS;
    }
}
