using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Threading;

namespace BordeX.Native
{
    public static class WindowManipulation
    {
        public static void SetWindowStyle(BorderType type, BorderType previousType, IntPtr windowHandle, Screen ChosenScreen, Taskbar taskbar, WindowStyles normalStyle, WindowStyles extendedStyle, WindowStyles normalStyleReplacement, WindowStyles extendedStyleReplacement, out Rectangle OriginalBounds)
        {
            OriginalBounds = new Rectangle(-int.MaxValue, -int.MaxValue, -int.MaxValue, -int.MaxValue);
            try
            {
                bool unrealDelay = WinAPI.GetWindowClassName(windowHandle).ToLower().Contains("unreal");

                if (previousType == BorderType.Restore)
                {
                    WinAPI.GetWindowRect(windowHandle, out RECT r);
                    OriginalBounds = new Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);
                }

                switch (type)
                {
                    case BorderType.Restore:
                        RestoreWindow(windowHandle, normalStyle, extendedStyle, OriginalBounds.X, OriginalBounds.Y, OriginalBounds.Width, OriginalBounds.Height, true);
                        break;
                    case BorderType.Borderless:
                        SetWindowProperties(windowHandle, normalStyleReplacement, extendedStyleReplacement, OriginalBounds.X, OriginalBounds.Y, OriginalBounds.Width, OriginalBounds.Height, unrealDelay, false);
                        break;
                    case BorderType.Maximized_Borderless:
                        SetWindowProperties(windowHandle, normalStyleReplacement, extendedStyleReplacement, ChosenScreen.Bounds.Location.X, ChosenScreen.Bounds.Location.Y, ChosenScreen.Bounds.Width, ChosenScreen.Bounds.Height - taskbar.Size.Height, unrealDelay, true);
                        break;
                    case BorderType.Fullscreen_Borderless:
                        SetWindowProperties(windowHandle, normalStyleReplacement, extendedStyleReplacement, ChosenScreen.Bounds.Location.X, ChosenScreen.Bounds.Location.Y, ChosenScreen.Bounds.Width, ChosenScreen.Bounds.Height, unrealDelay, true);
                        break;
                    case BorderType.All_Monitors_Maximised_Borderless:
                        // TODO: 
                        break;
                    case BorderType.All_Monitors_Fullscreen_Borderless:
                        // TODO: 
                        break;
                }
            } catch (Exception e)
            {
                // Make sure none of this crashes BordeX
                MessageBox.Show("There has been an error while trying to modify this window!\n\n" + e, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void SetWindowProperties(IntPtr handle, WindowStyles styles, WindowStyles extendedStyles, int x, int y, int width, int height, bool needsDelay = false, bool maximize = false)
        {
            if (needsDelay)
            {
                if (maximize) WinAPI.ShowWindow(handle, WindowShowStyle.Maximize);
                WinAPI.SetWindowPos(handle, 0, x, y, width, height, WindowPositionFlags.ShowWindow | WindowPositionFlags.NoOwnerZOrder | WindowPositionFlags.NoSendChanging);
                Thread.Sleep(TimeSpan.FromSeconds(4D));
                WinAPI.SetWindowLong(handle, WindowLongIndex.Style, styles);
                WinAPI.SetWindowLong(handle, WindowLongIndex.ExtendedStyle, extendedStyles);
            } else
            {
                WinAPI.SetWindowLong(handle, WindowLongIndex.Style, styles);
                WinAPI.SetWindowLong(handle, WindowLongIndex.ExtendedStyle, extendedStyles);
                if (maximize) WinAPI.ShowWindow(handle, WindowShowStyle.Maximize);
                WinAPI.SetWindowPos(handle, 0, x, y, width, height, WindowPositionFlags.ShowWindow | WindowPositionFlags.NoOwnerZOrder | WindowPositionFlags.NoSendChanging);
            }
        }

        private static void RestoreWindow(IntPtr handle, WindowStyles NormalStyle, WindowStyles ExtendedStyle, int x, int y, int width, int height, bool restoreBounds = false)
        {
            if (restoreBounds) WinAPI.SetWindowPos(handle, 0, x, y, width, height, WindowPositionFlags.ShowWindow | WindowPositionFlags.NoOwnerZOrder | WindowPositionFlags.NoSendChanging);

            WindowStyles winstyle = WinAPI.GetWindowLong(handle, WindowLongIndex.Style);
            WindowStyles winstyleExtended = WinAPI.GetWindowLong(handle, WindowLongIndex.ExtendedStyle);

            WinAPI.SetWindowLong(handle, WindowLongIndex.Style, NormalStyle);
            WinAPI.SetWindowLong(handle, WindowLongIndex.ExtendedStyle, ExtendedStyle);
        }

        // Get window thumbnail

        public static Bitmap PrintWindow(IntPtr hwnd)
        {
            WinAPI.GetWindowRect(hwnd, out RECT r);
            Bitmap bmp = new Bitmap(r.right - r.left, r.bottom - r.top);
            bmp.SetResolution(640, 360);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();
            bool succeeded = WinAPI.PrintWindow(hwnd, hdcBitmap, 0);
            gfxBmp.ReleaseHdc(hdcBitmap);
            if (!succeeded)
            {
                gfxBmp.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(Point.Empty, bmp.Size));
            }
            IntPtr hRgn = WinAPI.CreateRectRgn(0, 0, 0, 0);
            WinAPI.GetWindowRgn(hwnd, hRgn);
            Region region = Region.FromHrgn(hRgn);
            if (!region.IsEmpty(gfxBmp))
            {
                gfxBmp.ExcludeClip(region);
                gfxBmp.Clear(Color.Transparent);
            }
            gfxBmp.Dispose();
            return bmp;
        }

        public static bool IsMinimized(IntPtr hwnd)
        {
            try
            {
                WINDOWPLACEMENT win = new WINDOWPLACEMENT();
                WinAPI.GetWindowPlacement(hwnd, ref win);
                if (win.showCmd == 2) return true;
                else return false;
            } catch (Exception)
            {
                return true;
            }
        }

        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = WinAPI.GetForegroundWindow();

            if (WinAPI.GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }

    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public Point ptMinPosition;
        public Point ptMaxPosition;
        public Rectangle rcNormalPosition;
    }
}
