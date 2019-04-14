using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Reflection;
using System.IO;

using BordeX.Windows;

using Color = System.Drawing.Color;

namespace BordeX.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            AlwaysOnTopCheckBox.IsChecked = BaseWindow.Settings.Is_Always_Ontop;
            CanStartOnStartupCheckbox.IsChecked = BaseWindow.Settings.Auto_Startup;
            UseAccenColourCheckbox.IsChecked = BaseWindow.Settings.Use_Windows_Accent;
            RValueBox.Text = BaseWindow.Settings.Accent_Colour[0].ToString();
            GValueBox.Text = BaseWindow.Settings.Accent_Colour[1].ToString();
            BValueBox.Text = BaseWindow.Settings.Accent_Colour[2].ToString();
            ColourView.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
            EnableStartupSoundCheckBox.IsChecked = BaseWindow.Settings.Start_Sound_Enabled;
        }

        void StopApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            Program.CloseWindow(true);
        }

        void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(References.ForumPage);
        }

        void CanStartOnStartupCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                RegistryUtils.SetAutoStartup(true);
                BaseWindow.Settings.Auto_Startup = true;
                SettingsHandler.SaveSettings();
            }
        }

        void CanStartOnStartupCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                RegistryUtils.SetAutoStartup(false);
                BaseWindow.Settings.Auto_Startup = false;
                SettingsHandler.SaveSettings();
            }
        }

        void UseAccenColourCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                ColourView.Fill = SystemParameters.WindowGlassBrush;
                Common.Window.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BaseWindow.BackgroundAlpha, SystemParameters.WindowGlassColor.R, SystemParameters.WindowGlassColor.G, SystemParameters.WindowGlassColor.B));
                BaseWindow.Settings.Use_Windows_Accent = true;
                SettingsHandler.SaveSettings();
            }
        }

        void UseAccenColourCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                ColourView.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                Common.Window.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BaseWindow.BackgroundAlpha, BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                BaseWindow.Settings.Use_Windows_Accent = false;
                SettingsHandler.SaveSettings();
            }
        }

        void AlwaysOnTopCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                Common.Window.Topmost = true;
                BaseWindow.Settings.Auto_Startup = true;
                SettingsHandler.SaveSettings();
            }
        }

        void AlwaysOnTopCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                Common.Window.Topmost = false;
                BaseWindow.Settings.Auto_Startup = false;
                SettingsHandler.SaveSettings();
            }
        }

        void OpenBordeXFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", FileIO.AppDataDirectory);
        }

        void OpenBordeXInstallationFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        void EnableStartupSoundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                BaseWindow.Settings.Start_Sound_Enabled = true;
                SettingsHandler.SaveSettings();
            }
        }

        void EnableStartupSoundCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                BaseWindow.Settings.Start_Sound_Enabled = false;
                SettingsHandler.SaveSettings();
            }
        }

        void RValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (string.IsNullOrWhiteSpace(RValueBox.Text))
                {

                }

                BaseWindow.Settings.Accent_Colour[0] = byte.Parse(RValueBox.Text);
                ColourView.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                Common.Window.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BaseWindow.BackgroundAlpha, BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                SettingsHandler.SaveSettings();
            }
        }

        void GValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                BaseWindow.Settings.Accent_Colour[1] = byte.Parse(GValueBox.Text);
                ColourView.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                Common.Window.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BaseWindow.BackgroundAlpha, BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                SettingsHandler.SaveSettings();
            }
        }

        void BValueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                BaseWindow.Settings.Accent_Colour[2] = byte.Parse(BValueBox.Text);
                ColourView.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                Common.Window.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(BaseWindow.BackgroundAlpha, BaseWindow.Settings.Accent_Colour[0], BaseWindow.Settings.Accent_Colour[1], BaseWindow.Settings.Accent_Colour[2]));
                SettingsHandler.SaveSettings();
            }
        }

        private void SnapToTaskbarCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void SnapToTaskbarCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void UseDefaultRetryAmountCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void UseDefaultRetryAmountCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void AmountOfReTriesText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
