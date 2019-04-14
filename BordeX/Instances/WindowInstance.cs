using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Threading;

using BordeX.Pages;
using BordeX.Controls;
using BordeX.Native;

namespace BordeX.Profiles
{
    internal class WindowInstance
    {
        internal Process InstanceProcess
        {
            get
            {
                try
                {
                    return Process.GetProcessById(ProcessID);
                } catch (Exception)
                {
                    if (WindowInstanceManager.SelectedWindowInstance.ProcessID.Equals(ProcessID)) Common.Window.SetCurrentView(NavigationButtons.Window_Selection);
                    return null;
                }
            }
        }

        internal WindowInstanceSaveProfile SaveProfile
        {
            get
            {
                return WindowInstanceManager.SaveProfileContainer.Profiles[WinAPI.GetWindowClassName(InstanceProcess.MainWindowHandle)];
            }

            set
            {
                NormalStyle = WinAPI.GetWindowLong(InstanceProcess.MainWindowHandle, WindowLongIndex.Style);
                ExtendedStyle = WinAPI.GetWindowLong(InstanceProcess.MainWindowHandle, WindowLongIndex.ExtendedStyle);

                NormalStyleReplacement = (NormalStyle & ~(WindowStyles.WS_CAPTION | WindowStyles.WS_THICKFRAME | WindowStyles.WS_SYSMENU | WindowStyles.WS_MAXIMIZEBOX | WindowStyles.WS_MAXIMIZEBOX));
                ExtendedStyleReplacement = (ExtendedStyle & ~(WindowStyles.WS_EX_DLGMODALFRAME | WindowStyles.WS_EX_COMPOSITED | WindowStyles.WS_EX_WINDOWEDGE | WindowStyles.WS_EX_CLIENTEDGE | WindowStyles.WS_EX_LAYERED | WindowStyles.WS_EX_STATICEDGE | WindowStyles.WS_EX_TOOLWINDOW | WindowStyles.WS_EX_APPWINDOW));

                WindowInstanceManager.SaveProfileContainer.Profiles.Add(WinAPI.GetWindowClassName(InstanceProcess.MainWindowHandle), value);
            }
        }

        internal ProcessInstanceListItem ListItem { private set; get; }

        internal int ProcessID { private set; get; }
        internal bool IsAdminProcess { private set; get; }
        internal bool Is64bit { private set; get; }
        internal WindowGDI GDI { private set; get; }

        internal WindowStyles NormalStyleReplacement { private set; get; }
        internal WindowStyles ExtendedStyleReplacement { private set; get; }
        internal WindowStyles NormalStyle { private set; get; }
        internal WindowStyles ExtendedStyle { private set; get; }

        internal WindowInstance(int ProcessID, bool IsAdminProcess, bool Is64bit, WindowGDI GDI)
        {
            this.ProcessID = ProcessID;
            this.IsAdminProcess = IsAdminProcess;
            this.Is64bit = Is64bit;
            this.GDI = GDI;
            
            AsyncConstruction();
        }

        public async void AsyncConstruction()
        {
            if (Common.Window != null)
            {
                await Common.Window.Dispatcher.InvokeAsync(() =>
                {
                    ListItem = new ProcessInstanceListItem(this, InstanceProcess);
                    if (Common.Window.ContentFrame.Content.GetType() == typeof(WindowSelectionPage)) (Common.Window.ContentFrame.Content as WindowSelectionPage).OpenWindowsList.Children.Add(ListItem);
                }, DispatcherPriority.Background);
            }
        }
    }

    public class WindowInstanceSaveProfile
    {
        internal BorderType Prev_Border = BorderType.Restore;
        BorderType B = BorderType.Restore;
        public BorderType Border
        {
            get
            {
                return B;
            }

            set
            {
                Prev_Border = B;
                B = value;
                if (Common.Window != null)
                {
                    Common.Window.Dispatcher.InvokeAsync(() =>
                    {
                        if (Common.Window.ContentFrame.Content.GetType() == typeof(ConfigurationPage))
                        {
                            WindowManipulation.SetWindowStyle(
                            WindowInstanceManager.SelectedWindowInstance.SaveProfile.Border, WindowInstanceManager.SelectedWindowInstance.SaveProfile.Prev_Border,
                            WindowInstanceManager.SelectedWindowInstance.InstanceProcess.MainWindowHandle, WindowInstanceManager.SelectedWindowInstance.SaveProfile.ChosenScreen,
                            Common.TaskbarInstance, WindowInstanceManager.SelectedWindowInstance.NormalStyle, WindowInstanceManager.SelectedWindowInstance.ExtendedStyle,
                            WindowInstanceManager.SelectedWindowInstance.NormalStyleReplacement, WindowInstanceManager.SelectedWindowInstance.ExtendedStyleReplacement, out Rectangle OriginalBounds);
                            if (OriginalBounds.X != -int.MaxValue && OriginalBounds.Y != -int.MaxValue && OriginalBounds.Width != -int.MaxValue && OriginalBounds.Height != -int.MaxValue) WindowInstanceManager.SelectedWindowInstance.SaveProfile.OriginalBounds = OriginalBounds;
                        }
                    });
                }
            }
        }
        public Screen ChosenScreen = Screen.PrimaryScreen;
        public Rectangle OriginalBounds { set; get; }
    }

    public class WindowInstanceSaveProfilesContainer
    {
        public Dictionary<string, WindowInstanceSaveProfile> Profiles = new Dictionary<string, WindowInstanceSaveProfile>();
    }
}
