using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BordeX.Native
{
    public static class KeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void Hook()
        {
            _hookID = SetHook(_proc);
        }

        public static void Unhook()
        {
            WinAPI.UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return WinAPI.SetWindowsHookEx(WH_KEYBOARD_LL, proc, WinAPI.GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        internal delegate IntPtr LowLevelKeyboardProc( int nCode, IntPtr wParam, IntPtr lParam);
        
        private static bool IsALTDown = false;
        private static IntPtr HookCallback( int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (IsALTDown && ((Keys)vkCode).Equals(Keys.P))
                {
                    // TODO: Listen into this so i can execute code elsewhere
                    /*Common.Window.Dispatcher.InvokeAsync(() =>
                    {
                        WindowManipulation.SetWindowStyle(WindowInstanceManager.Windows.First(x => x.InstanceProcess.MainWindowTitle.Equals(WindowManipulation.GetActiveWindowTitle())));
                    });*/
                }
                IsALTDown = ((Keys)vkCode).Equals(Keys.Alt);
            }
            return WinAPI.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
