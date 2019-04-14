using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BordeX.Forms.Controls
{
    public partial class ProcessProfileListItem : UserControl
    {
        public ProcessProfile profile;

        public ProcessProfileListItem(ProcessProfile profile)
        {
            InitializeComponent();

            this.profile = profile;
            windowIcon.Image = profile.WindowIcon;
            windowTitle.Text = profile.WindowTitle;
        }

        private void windowTitle_Click(object sender, EventArgs e)
        {
            Program.MainForm.ChangeSelectedProcess(this);
        }
    }
}
