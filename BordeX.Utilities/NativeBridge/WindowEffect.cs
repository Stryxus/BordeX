using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace BordeX.Native
{
    public static class WindowEffect
    {
        public static void BlurWindow(Window win, AccentState state)
        {
            AccentPolicy accent = new AccentPolicy();
            accent.AccentState = state;
            int accentStructSize = Marshal.SizeOf(accent);
            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);
            WindowCompositionAttributeData data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;
            WinAPI.SetWindowCompositionAttribute(new WindowInteropHelper(win).Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
}
