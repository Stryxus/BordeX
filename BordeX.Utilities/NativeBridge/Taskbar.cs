using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace BordeX.Native
{
    public sealed class Taskbar
    {
        private const string ClassName = "Shell_TrayWnd";
        public Rectangle Bounds { get; private set; }
        public TaskbarPosition Position { get; private set; }
        public Point Location { get { return Bounds.Location; } }
        public Size Size { get { return Bounds.Size; } }
        public bool AlwaysOnTop { get; private set; }
        public bool AutoHide { get; private set; }
        public IntPtr Handle { get; private set; }

        public Taskbar()
        {
            Handle = WinAPI.FindWindow(ClassName, null);

            APPBARDATA data = new APPBARDATA();
            data.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
            data.hWnd = Handle;
            IntPtr result = WinAPI.SHAppBarMessage(ABM.GetTaskbarPos, ref data);
            if (result == IntPtr.Zero) throw new InvalidOperationException();

            Position = (TaskbarPosition)data.uEdge;
            Bounds = Rectangle.FromLTRB(data.rc.left, data.rc.top, data.rc.right, data.rc.bottom);

            data.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
            result = WinAPI.SHAppBarMessage(ABM.GetState, ref data);
            int state = result.ToInt32();
            AlwaysOnTop = (state & ABS.AlwaysOnTop) == ABS.AlwaysOnTop;
            AutoHide = (state & ABS.Autohide) == ABS.Autohide;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public ABE uEdge;
        public RECT rc;
        public int lParam;
    }

    public static class ABS
    {
        public const int Autohide = 0x0000001;
        public const int AlwaysOnTop = 0x0000002;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
