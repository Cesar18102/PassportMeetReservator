using System;
using System.Windows.Forms;

namespace PassportMeetReservator.Forms
{
    public partial class ZoomedBrowserForm : Form
    { 
        private void ZoomedBrowserForm_ResizeEnd(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
                control.Size = this.Size;
        }
    }
}
