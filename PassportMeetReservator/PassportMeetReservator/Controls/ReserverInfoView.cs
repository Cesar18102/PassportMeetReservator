using System.Windows.Forms;
using System.ComponentModel;

namespace PassportMeetReservator.Controls
{
    public class ReserverInfoView : GroupBox
    {
        [Category("Inputs")]
        public NamedInput UrlInput { get; set; }
    }
}
