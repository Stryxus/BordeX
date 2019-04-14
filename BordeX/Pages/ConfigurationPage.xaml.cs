using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Timers;

using BordeX.Native;
using BordeX.Profiles;

namespace BordeX.Pages
{
    public partial class ConfigurationPage : Page
    {
        private static Timer t = new Timer();
        private static bool IsWindowMinimized = true;

        public ConfigurationPage()
        {
            InitializeComponent();

            // Essentially i want to find a way to capture a window live while hardly using the CPU or not using it at all (GPU Accelerated)
            // Capture the DirectX/OpenGL/Vulkan back buffer?
            // May try to re-engineer this for my needs -> https://github.com/hecomi/uDesktopDuplication
            t.Elapsed += ((object sender, ElapsedEventArgs args) => 
            {
                Common.Window.Dispatcher.InvokeAsync((() => 
                {
                    try
                    {
                        if (WindowManipulation.IsMinimized(WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainWindowHandle))
                        {
                            if (!IsWindowMinimized)
                            {
                                WindowVisualRepresentation.Source = new BitmapImage(new Uri("C:\\Windows\\Web\\Wallpaper\\Windows\\img0.jpg"));
                                IsWindowMinimized = true;
                            }
                        }
                        else
                        {
                            IsWindowMinimized = false;
                            try
                            {
                                WindowVisualRepresentation.Source = ImageConversion.ImageSourceForBitmap(WindowManipulation.PrintWindow(WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainWindowHandle));
                            }
                            catch (ArgumentException)
                            {
                                WindowVisualRepresentation.Source = new BitmapImage(new Uri("C:\\Windows\\Web\\Wallpaper\\Windows\\img0.jpg"));
                            }
                        }
                    } catch (Exception) { Common.Window.SetCurrentView(NavigationButtons.Window_Selection); }
                }));
            });
            Unloaded += ((object sender, RoutedEventArgs args) => 
            {
                t.Stop();
            });
            t.Interval = TimeSpan.FromSeconds(0.2D).TotalMilliseconds;
            t.Start();

            CurrentProfile.Text = WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainWindowTitle;

            Icon i = Icon.ExtractAssociatedIcon(WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainModule.FileName);
            if (i != null) AppIcon.Source = ImageConversion.ToImageSource(i);
            else AppIcon.Source = ImageConversion.ToImageSource(Icon.FromHandle(SystemIcons.Application.Handle));

            if (!WindowInstanceManager.SaveProfileContainer.Profiles.ContainsKey(WinAPI.GetWindowClassName(WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainWindowHandle))) WindowInstanceManager.SaveProfileContainer.Profiles.Add(WinAPI.GetWindowClassName(WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainWindowHandle), new WindowInstanceSaveProfile());
            WindowInstanceManager.SelectedWindowInstance.ListItem.HasProfileImage.Visibility = Visibility.Visible;
            FileIO.SaveProfiles(WindowInstanceManager.SaveProfileContainer);
        }

        void DeleteProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Common.Window.SetCurrentView(NavigationButtons.Window_Selection);
            WindowInstanceManager.SaveProfileContainer.Profiles.Remove(WinAPI.GetWindowClassName(WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainWindowHandle));
            WindowInstanceManager.SelectedWindowInstance.ListItem.HasProfileImage.Visibility = Visibility.Hidden;
            WindowInstanceManager.SaveProfiles();
        }
    }
}
