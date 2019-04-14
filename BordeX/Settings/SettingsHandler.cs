using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

using BordeX.Windows;

namespace BordeX
{
    static class SettingsHandler
    {
        internal static void LoadSettings()
        {
            BaseWindow.Settings = FileIO.LoadSettings<SettingsProfile>();
            if (BaseWindow.Settings.Use_Windows_Accent)
            {
                Common.Window.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BaseWindow.BackgroundAlpha, SystemParameters.WindowGlassColor.R, SystemParameters.WindowGlassColor.G, SystemParameters.WindowGlassColor.B));
            }
            else
            {
                Common.Window.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BaseWindow.BackgroundAlpha, BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
            }
            if (BaseWindow.Settings.Is_Always_Ontop) Common.Window.Topmost = true;
        }

        // Just have this here so i dont have to reference FileIO along with this
        internal static void SaveSettings()
        {
            FileIO.SaveSettings(BaseWindow.Settings);
        }
    }
}
