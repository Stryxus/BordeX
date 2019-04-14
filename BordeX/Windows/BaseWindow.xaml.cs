using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

using BordeX.Pages;
using BordeX.Profiles;

using Screen = System.Windows.Forms.Screen;

namespace BordeX.Windows
{
    public partial class BaseWindow : Window
    {
        internal static byte BackgroundAlpha = 150;
        internal static SettingsProfile Settings = new SettingsProfile();

        public BaseWindow()
        {
            InitializeComponent();

            Top = Screen.PrimaryScreen.Bounds.Height - (Common.TaskbarInstance.Bounds.Height + Height);
            Left = Screen.PrimaryScreen.Bounds.Width - Width;
            OpenWindowsNavButton.Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40));

            SystemParameters.StaticPropertyChanged += ((object e, PropertyChangedEventArgs args) =>
            {
                if (Settings.Use_Windows_Accent) Background = new SolidColorBrush(Color.FromArgb(BackgroundAlpha, SystemParameters.WindowGlassColor.R, SystemParameters.WindowGlassColor.G, SystemParameters.WindowGlassColor.B));
            });

            TitleText.Text = References.Name + " " + References.Version;

            SetCurrentView(NavigationButtons.Window_Selection);
        }

        internal void SetCurrentView(NavigationButtons button)
        {
            switch (button)
            {
                case NavigationButtons.News_And_Changelogs:
                    if (ContentFrame.Content == null || ContentFrame.Content.GetType() != typeof(NewsPage))
                    {
                        ContentFrame.Navigate(new NewsPage());
                        ResetButtons();
                        NewsNavButton.IsChecked = true;
                    }
                    break;
                case NavigationButtons.Window_Selection:
                    if (ContentFrame.Content == null || ContentFrame.Content.GetType() != typeof(WindowSelectionPage))
                    {
                        ContentFrame.Navigate(new WindowSelectionPage());
                        ResetButtons();
                        OpenWindowsNavButton.IsChecked = true;
                    }
                    break;
                case NavigationButtons.Window_Configuration:
                    if (ContentFrame.Content.GetType() != typeof(ConfigurationPage))
                    {
                        ContentFrame.Navigate(new ConfigurationPage());
                        ResetButtons();
                        ConfigurationNavButton.IsChecked = true;
                    }
                    break;
                case NavigationButtons.Performance:
                    MessageBox.Show("Performance Monitoring is still in development! Im sure you dont want this breaking your computer :D", "Coming Soon!", MessageBoxButton.OK, MessageBoxImage.Information);
                    /*if (ContentFrame.Content.GetType() != typeof(BenchmarkingPage))
                    {
                        ContentFrame.Navigate(new BenchmarkingPage());
                        ResetButtons();
                        BenchmarkingNavButton.IsChecked = true;
                    }*/
                    break;
                case NavigationButtons.BordeX_Settings:
                    if (ContentFrame.Content.GetType() != typeof(SettingsPage))
                    {
                        ContentFrame.Navigate(new SettingsPage());
                        ResetButtons();
                        BordeXOptionsNavButton.IsChecked = true;
                    }
                    break;
            }

            void ResetButtons()
            {
                NewsNavButton.IsChecked = false;
                OpenWindowsNavButton.IsChecked = false;
                ConfigurationNavButton.IsChecked = false;
                BordeXOptionsNavButton.IsChecked = false;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Program.CloseWindow();
        }

        private void HomeNavButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentView(NavigationButtons.News_And_Changelogs);
        }

        private void OpenWindowsNavButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentView(NavigationButtons.Window_Selection);
        }

        private void ConfigurationNavButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowInstanceManager.SelectedWindowInstance != null)
            {
                SetCurrentView(NavigationButtons.Window_Configuration);
            }
            else MessageBox.Show("No window has been selected for configuration!", "No Profile", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void PerformanceNavButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentView(NavigationButtons.Performance);
        }

        private void BordeXOptionsNavButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentView(NavigationButtons.BordeX_Settings);
        }

        private void StorePageButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(References.StorePage);
        }

        private void TwitterButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://twitter.com/Stryxus");
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void WebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://stryxus.co.uk/index.html?p=bordex");
        }
    }

    public enum PageButtons
    {
        News_And_Changelogs,
        Window_Selection,
        Window_Configuration,
        BordeX_Settings
    }
}