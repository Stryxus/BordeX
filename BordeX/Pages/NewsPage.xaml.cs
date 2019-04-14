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
using System.IO;
using System.Reflection;

namespace BordeX.Pages
{
    public partial class NewsPage : Page
    {
        public string FileText
        {
            get
            {
                using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("BordeX.Assets.news.yml"))
                {
                    using (StreamReader r = new StreamReader(s))
                    {
                        return r.ReadToEnd();
                    }
                }
            }
        }

        public NewsPage()
        {
            InitializeComponent();

            News.Text = FileText;
        }
    }
}
