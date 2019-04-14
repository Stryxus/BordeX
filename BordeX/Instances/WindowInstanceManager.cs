using System;
using System.Collections.Concurrent;
using System.Management;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;

using BordeX.Pages;
using BordeX.Security;
using BordeX.Native;

namespace BordeX.Profiles
{
    static class WindowInstanceManager
    {
        internal static ConcurrentList<WindowInstance> Windows = new ConcurrentList<WindowInstance>();
        internal static WindowInstanceSaveProfilesContainer SaveProfileContainer = new WindowInstanceSaveProfilesContainer();

        static WindowInstance SWI;
        internal static WindowInstance SelectedWindowInstance
        {
            get
            {
                return SWI;
            }

            set
            {
                SWI = value;
                Logger.LogInfo("Changed Window Profile -> " + value.InstanceProcess.MainWindowTitle);
                if (Common.Window.ContentFrame.Content.GetType() == typeof(WindowSelectionPage))
                {
                    if (value != null) (Common.Window.ContentFrame.Content as WindowSelectionPage).ConfigureButton.IsEnabled = true;
                    else (Common.Window.ContentFrame.Content as WindowSelectionPage).ConfigureButton.IsEnabled = false;
                }
            }
        }

        static ManagementEventWatcher addWatcher;
        static ManagementEventWatcher removeWatcher;
        internal static bool IsUpdaterInitialized = false;
        
        static int MaxRetries = 2;
        static int RetriesLeft = MaxRetries;

        static string[] Blacklist =
        {
            // Skip this app
            "bordex",

            // Skip Browsers
            "microsoftedgecp", "chrome", "iexplore", "safari", "firefox", 

            // Game Software
            "steam", "uplay", "origin",

            // System
            "taskmgr", "scriptedsandbox", "scriptedsandbox64", "explorer", "shellexperiencehost", "applicationframehost", "svchost", "system"
        };

        internal static void BeginUpdater()
        {
            foreach (Process p in Process.GetProcesses()) AddWindow(p);

            addWatcher = new ManagementEventWatcher();
            addWatcher.Query = new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace");
            addWatcher.Start();
            addWatcher.EventArrived += ((object sender, EventArrivedEventArgs args) =>
            {
                DoEvent();
                void DoEvent(bool isRetry = false)
                {
                    try
                    {
                        AddWindow(Process.GetProcessById(int.Parse(args.NewEvent.Properties["ProcessId"].Value.ToString())), true);
                    }
                    catch (ArgumentException)
                    {
                        Logger.LogWarn("Picked up process that closed immediatly [RETRY]");
                        Retry();
                    }

                    void Retry()
                    {
                        if (RetriesLeft != 0)
                        {
                            if (RetriesLeft == MaxRetries || isRetry) RetriesLeft--;
                            DoEvent(true);
                        }
                        else RetriesLeft = MaxRetries;
                    }
                }
            });

            removeWatcher = new ManagementEventWatcher();
            removeWatcher.Query = new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace");
            removeWatcher.Start();
            removeWatcher.EventArrived += ((object sender, EventArrivedEventArgs args) => 
            {
                WindowInstance instance = Windows.FirstOrDefault(x => x.ProcessID.Equals(int.Parse(args.NewEvent.Properties["ProcessId"].Value.ToString())));
                if (instance != null)
                {
                    if (!instance.Equals(default(WindowInstance))) RemoveWindow(instance);
                }
            });
            IsUpdaterInitialized = true;
        }

        internal static void StopUpdater()
        {
            addWatcher.Stop();
            addWatcher.Dispose();
            removeWatcher.Stop();
            removeWatcher.Dispose();
        }

        static void AddWindow(Process p, bool delay = false, bool isRetry = false)
        {
            Task.Run(async () => 
            {
                if (delay) await Task.Delay(TimeSpan.FromSeconds(4D));

                try
                {
                    if (!Windows.Any(x => x.ProcessID.Equals(p.Id)) && !Blacklist.Contains(p.ProcessName.ToLower()))
                    {
                        WindowGDI GDI = WindowGDI.None;

                        ProcessModule[] modules = new ProcessModule[p.Modules.Count];
                        p.Modules.CopyTo(modules, 0);

                        bool IsDirectX = modules.Any(x => x.FileName.ToLower().Contains("d3d"));
                        bool IsOpenGL = modules.Any(x => x.FileName.ToLower().Contains("opengl32"));

                        if (IsDirectX && IsOpenGL) GDI = WindowGDI.Hybrid;
                        else if (IsDirectX && !IsOpenGL) GDI = WindowGDI.DirectX;
                        else if (!IsDirectX && IsOpenGL) GDI = WindowGDI.OpenGL;
                        // Not sure why this doesnt work - Vulkan shows no DLL presence?
                        else if (modules.Any(x => x.FileName.ToLower().Contains("vulkan"))) GDI = WindowGDI.Vulkan;

                        if (WinAPI.IsWindow(p.MainWindowHandle))
                        {
                            Windows.Add(new WindowInstance(p.Id, UAC.IsProcessElevated(p.Handle), ProcessUtils.IsInWin64Emulator(p), GDI));
                            Logger.LogInfo("Adding Process with Window -> '" + p.MainWindowTitle + "' : [" + p.ProcessName + " | " + p.Id + "]");
                        }
                    }
                }
                catch (Win32Exception)
                {
                    Logger.LogError("Current iterated process is inaccessible! [PASS]");
                    return;
                }
                catch (InvalidOperationException e)
                {
                    Logger.LogWarn("Detected process that closed immediatly [RETRY]");
                    Retry();
                }

                void Retry()
                {
                    if (RetriesLeft != 0)
                    {
                        if (RetriesLeft == MaxRetries || isRetry) RetriesLeft--;
                        p.Refresh();
                        AddWindow(p, delay, true);
                    }
                    else RetriesLeft = MaxRetries;
                }
            });
        }

        static void RemoveWindow(WindowInstance instance)
        {
            Task.Run(async () => 
            {
                Logger.LogInfo("Removing Window Process -> [" + instance.ProcessID + "]");

                if (SelectedWindowInstance != null)
                {
                    if (SelectedWindowInstance.InstanceProcess.Id.Equals(instance.ProcessID))
                    {
                        await Common.Window.Dispatcher.InvokeAsync(() => { Common.Window.SetCurrentView(NavigationButtons.Window_Selection); });
                        SelectedWindowInstance = null;
                    }
                }

                if (Common.Window != null)
                {
                    await Common.Window.Dispatcher.InvokeAsync(() =>
                    {
                        if (Common.Window.ContentFrame.Content is WindowSelectionPage) (Common.Window.ContentFrame.Content as WindowSelectionPage).OpenWindowsList.Children.Remove(instance.ListItem);
                    }, DispatcherPriority.Background);
                }

                Windows.Remove(instance);
            });
        }

        internal static void SaveProfiles()
        {
            FileIO.SaveProfiles(SaveProfileContainer.Profiles);
        }
    }
}
