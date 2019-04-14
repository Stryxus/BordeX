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
using BordeX.Pages;

namespace BordeX.Controls
{
    public partial class OpenWindowsFilter : UserControl
    {
        public OpenWindowsFilter()
        {
            InitializeComponent();

            foreach (string gdi in Enum.GetNames(typeof(WindowGDI))) GITypeFilter.Items.Add(gdi);
        }

        private void SearchFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            (Common.Window.ContentFrame.Content as WindowSelectionPage).FilterControlList();
        }

        private void GITypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Common.Window != null)
            {
                if (Common.Window.ContentFrame.Content.GetType() == typeof(WindowSelectionPage)) (Common.Window.ContentFrame.Content as WindowSelectionPage).FilterControlList();
            }
        }
    }
}
