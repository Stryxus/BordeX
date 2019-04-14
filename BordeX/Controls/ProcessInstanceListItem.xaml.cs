using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Timers;
using System.Drawing;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using BordeX.Native;
using BordeX.Pages;
using BordeX.Profiles;

using Color = System.Windows.Media.Color;

namespace BordeX.Controls
{
    public partial class ProcessInstanceListItem : UserControl
    {
        internal int ProcessID;
        internal bool IsSelected = false;
        bool hasprofile = false;
        internal bool HasProfile
        {
            get
            {
                return hasprofile;
            }

            set
            {
                hasprofile = value;
                HasProfileImage.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }
        
        internal ProcessInstanceListItem(WindowInstance pro, Process process)
        {
            InitializeComponent();
            Main.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));

            MouseDown += ((object sender, MouseButtonEventArgs args) => 
            {
                if (args.LeftButton == MouseButtonState.Pressed) (Common.Window.ContentFrame.Content as WindowSelectionPage).ChangeSelectedProcess(this);
            });

            MouseDoubleClick += ((object sender, MouseButtonEventArgs args) => 
            {
                if (args.LeftButton == MouseButtonState.Pressed)
                {
                    (Common.Window.ContentFrame.Content as WindowSelectionPage).ChangeSelectedProcess(this);
                    Common.Window.SetCurrentView(NavigationButtons.Window_Configuration);
                }
            });

            ProcessID = process.Id;

            try
            {
                Icon i = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
                if (i != null) ProcessIcon.Source = ImageConversion.ToImageSource(i);
                else ProcessIcon.Source = ImageConversion.ToImageSource(Icon.FromHandle(SystemIcons.Application.Handle));
            } catch (Win32Exception) { }

            if (WindowInstanceManager.SaveProfileContainer.Profiles.ContainsKey(WinAPI.GetWindowClassName(pro.InstanceProcess.MainWindowHandle))) HasProfileImage.Visibility = Visibility.Visible;
            if (pro.IsAdminProcess) IsAdminImage.Visibility = Visibility.Visible;
            ProcessName.Text = (!string.IsNullOrEmpty(pro.InstanceProcess.MainWindowTitle) && !string.IsNullOrWhiteSpace(pro.InstanceProcess.MainWindowTitle)) ? pro.InstanceProcess.MainWindowTitle : pro.InstanceProcess.ProcessName;
            ProcessGIType.Text = pro.GDI.ToString();
            ProcessIconBackground.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));

            Timer t = new Timer();
            t.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
            t.Elapsed += (async (object sender, ElapsedEventArgs args) => 
            {
                await Common.Window.Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        pro.InstanceProcess.Refresh();
                        ProcessName.Text = (!string.IsNullOrEmpty(pro.InstanceProcess.MainWindowTitle) && !string.IsNullOrWhiteSpace(pro.InstanceProcess.MainWindowTitle)) ? pro.InstanceProcess.MainWindowTitle : pro.InstanceProcess.ProcessName;
                    }
                    catch (Exception) { }
                });
            });
            t.Start();
        }
    }
}