using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using BordeX.Controls;
using BordeX.Profiles;

namespace BordeX.Pages
{
    public partial class WindowSelectionPage : Page
    {
        internal List<WindowInstance> FilteredProfileControls
        {
            get
            {
                List<WindowInstance> filtered;
                
                filtered = WindowInstanceManager.Windows.Where(x => x.GDI == ((WindowGDI)FilterControl.GITypeFilter.SelectedIndex) || ((WindowGDI)FilterControl.GITypeFilter.SelectedIndex) == WindowGDI.All).ToList();
                filtered = filtered.Where(x => x.InstanceProcess.MainWindowTitle.ToLower().Contains(FilterControl.SearchFilter.Text.ToLower()) || FilterControl.SearchFilter.Text.Length == 0).ToList();

                return filtered;
            }
        }

        public WindowSelectionPage()
        {
            InitializeComponent();
            
            TopMessage.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
        }

        internal void ChangeSelectedProcess(ProcessInstanceListItem item)
        {
            foreach (WindowInstance i in WindowInstanceManager.Windows)
            {
                if (i.InstanceProcess.Id.Equals(item.ProcessID)) WindowInstanceManager.SelectedWindowInstance = i;
            }

            foreach (ProcessInstanceListItem i in OpenWindowsList.Children)
            {
                i.IsSelected = false;
                i.Main.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            }
            item.Main.Background = SystemParameters.WindowGlassBrush;
        }

        private void ConfigureButton_Click(object sender, RoutedEventArgs e)
        {
            Common.Window.SetCurrentView(NavigationButtons.Window_Configuration);
        }

        private void FiltersButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterControl.Visibility == Visibility.Visible) FilterControl.Visibility = Visibility.Hidden;
            else FilterControl.Visibility = Visibility.Visible;
        }

        internal void FilterControlList()
        {
            OpenWindowsList.Children.Clear();
            foreach (WindowInstance instance in FilteredProfileControls) OpenWindowsList.Children.Add(instance.ListItem);
        }

        private void OpenWindowsList_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindowInstanceManager.IsUpdaterInitialized)
            {
                try
                {
                    foreach (WindowInstance instance in WindowInstanceManager.Windows) OpenWindowsList.Children.Add(instance.ListItem);
                } catch (ArgumentNullException) { }
            }
        }

        private void OpenWindowsList_Unloaded(object sender, RoutedEventArgs e)
        {
            OpenWindowsList.Children.Clear();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindowsList.Children.Clear();
            await Task.Delay(TimeSpan.FromSeconds(0.25D));
            try
            {
                foreach (WindowInstance instance in WindowInstanceManager.Windows) OpenWindowsList.Children.Add(instance.ListItem);
            }
            catch (ArgumentNullException) { }
        }
    }
}
