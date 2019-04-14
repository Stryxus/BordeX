using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Media;

using BordeX.Native;
using BordeX.Windows;
using BordeX.Profiles;

using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace BordeX
{
    public partial class Program : Application
    {
        [STAThread]
        static void Main()
        {
            new SplashScreen("splash.png").Show(true);
            FileIO.ValidateFileSystem();

            Logger.LogInfo("Initializing BordeX");

            Common.TaskbarInstance = new Taskbar();
            Common.ProgramInstance = new Program();
            Common.ProgramInstance.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Common.Window = new BaseWindow();

            Common.NotificationIcon = new NotifyIcon();
            Common.NotificationIcon.Icon = BordeX.Properties.Resources.logo;
            Common.NotificationIcon.Text = References.Name;
            Common.NotificationIcon.BalloonTipText = References.Name;

            Common.NotificationIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            Common.NotificationIcon.Click += ((object sender, EventArgs arg) => { OpenWindow(); });
            Common.NotificationIcon.ContextMenu.MenuItems.Add("Open/Show Window", (object sender, EventArgs e) => { OpenWindow(); });
            Common.NotificationIcon.ContextMenu.MenuItems.Add("Close Window", (object sender, EventArgs e) => { CloseWindow(); });
            Common.NotificationIcon.ContextMenu.MenuItems.Add("-");
            Common.NotificationIcon.ContextMenu.MenuItems.Add("Shutdown Application", (object sender, EventArgs e) => { CloseWindow(true); });
            Common.NotificationIcon.Visible = true;

            if (!WMI.IsWindows10)
            {
                // Need to figure out a way to keep this messagebox open without RUN
                Logger.LogError("You're current Operating System is not supported by BordeX or something is blocking access to Operating System information!\n\nPlease upgrade to either Windows 10 (Recommended), Windows 8.1 or Windows 7.", true);
                return;
            }
            
            WindowInstanceManager.BeginUpdater();
            SettingsHandler.LoadSettings();
            WindowInstanceManager.SaveProfileContainer = FileIO.LoadProfiles<WindowInstanceSaveProfilesContainer>();
#if !DEBUG
            //Steam.Initialize(Process.GetCurrentProcess().Id);
#endif
            OpenWindow();
            if (BaseWindow.Settings.Start_Sound_Enabled) new SoundPlayer("C:\\Windows\\Media\\Windows Unlock.wav").Play();
            KeyboardListener.Hook();
            Common.ProgramInstance.Run(Common.Window);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            WindowInstanceManager.StopUpdater();
            Common.NotificationIcon.Dispose();
#if !DEBUG
            //Steam.Shutdown();
#endif
            KeyboardListener.Unhook();
            //new SoundPlayer("C:\\Windows\\Media\\Speech Sleep.wav").PlaySync();
            base.OnExit(e);
        }

        internal static void OpenWindow()
        {
            if (Common.Window.Visibility != Visibility.Visible)
            {
                Common.Window.Show();
                Common.Window.Opacity = 0D;
                Common.Window.Top = Common.Window.Top + 100;
                WindowEffect.BlurWindow(Common.Window, AccentState.ACCENT_ENABLE_BLURBEHIND);
                Common.Window.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromSeconds(0.2D)));
                Common.Window.BeginAnimation(Window.TopProperty, new DoubleAnimation(Common.Window.Top - 100, TimeSpan.FromSeconds(0.4D)));
            } else Common.Window.Focus();
        }

        internal static void CloseWindow(bool shutdown = false)
        {
            DoubleAnimation an = new DoubleAnimation(0D, TimeSpan.FromSeconds(0.2D));
            DoubleAnimation an2 = new DoubleAnimation(Common.Window.Top + 100, TimeSpan.FromSeconds(0.2D));
            bool a1 = false;
            bool a2 = false;
            an.Completed += (e, args) =>
            {
                a1 = true;
                CallComplete();
            };
            an2.Completed += (e, args) =>
            {
                a2 = true;
                CallComplete();
            };
            Common.Window.BeginAnimation(UIElement.OpacityProperty, an);
            Common.Window.BeginAnimation(Window.TopProperty, an2);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            void CallComplete()
            {
                if (a1 && a2)
                {
                    Common.Window.Hide();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (shutdown) Common.ProgramInstance.Shutdown();
                }
            }
        }
    }
}
