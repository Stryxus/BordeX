using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

using BordeX.Forms.Controls;

namespace BordeX.Forms
{
    public partial class ControllerForm : Form
    {
        public ProcessProfile SelectedProfile;

        public ControllerForm()
        {
            InitializeComponent();

#if !DEBUG
            Steam.Initialize(Process.GetCurrentProcess().Id);
#endif
            Text = Program.References.Name + " " + Program.References.Version;
            processList.HorizontalScroll.Visible = false;
            processProfileList.HorizontalScroll.Visible = false;

            if (!FileIO.Exists(FileIO.DesktopScreenshotsDirectory + "m_1.jpg")) ScreenshotDesktops();
            desktopViewer.Image = FileIO.LoadImage(FileIO.DesktopScreenshotsDirectory + "m_1.jpg");
        }

        internal void ChangeSelectedProcess(ProcessProfileListItem item)
        {
            try
            {
                processViewingText.Text = "Profile: " + item.profile.WindowTitle;
                processViewingIcon.Image = item.profile.WindowIcon;
                foreach (ProcessProfileListItem i in processList.Controls) i.BackColor = Color.Transparent;
                item.BackColor = Color.CornflowerBlue;
                SelectedProfile = item.profile;
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Window '" + item.profile.WindowTitle + "' no longer exists. Refreshing list...", "Window not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshProcessList();
            }
        }

        internal void RefreshProcessList()
        {
            processList.Controls.Clear();
            foreach (ProcessProfile p in ProcessProfileHandler.Processes) processList.Controls.Add(new ProcessProfileListItem(p));
        }

        internal void ScreenshotDesktops()
        {
            Task.Run(() => 
            {


                FileIO.Delete(FileIO.DesktopScreenshotsDirectory);
                FileIO.Create(FileIO.DesktopScreenshotsDirectory);
                WinAPI.MinimizeAll();
                Thread.Sleep(TimeSpan.FromSeconds(0.25D));
                int i = 1;
                foreach (MonitorInstance r in MonitorInstanceHandler.Monitors)
                {
                    Graphics g = CreateGraphics();
                    Bitmap memoryImage = new Bitmap(r.Resolution.Width, r.Resolution.Height, g);
                    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                    memoryGraphics.CopyFromScreen(r.Location.X, r.Location.Y, 0, 0, r.Resolution);
                    FileIO.SaveImage(FileIO.DesktopScreenshotsDirectory + "m_" + i + ".jpg", memoryImage, ImageFormat.Jpeg);
                    i++;
                }
                Thread.Sleep(TimeSpan.FromSeconds(0.25D));
                WinAPI.UndoMinimizeAll();
            });
        }

        private void refreshOpenWindowListMenuItem_Click(object sender, EventArgs e)
        {
            RefreshProcessList();
        }

        private void testStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinAPI.SetWindowStyle(SelectedProfile);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinAPI.SetWindowStyle(SelectedProfile);
        }

        private void refreshDesktopScreenshotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenshotDesktops();
        }

        private void borderlessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedProfile.Resize_Req = ResizeRequest.BORDERLESS;
            WinAPI.SetWindowStyle(SelectedProfile);
        }

        private void maximizedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedProfile.Resize_Req = ResizeRequest.MAXIMIZED_BORDERLESS;
            WinAPI.SetWindowStyle(SelectedProfile);
        }

        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedProfile.Resize_Req = ResizeRequest.FULLSCREEN_BORDERLESS;
            WinAPI.SetWindowStyle(SelectedProfile);
        }
    }
}
