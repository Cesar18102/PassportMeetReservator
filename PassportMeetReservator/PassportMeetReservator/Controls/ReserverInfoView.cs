using System.Windows.Forms;
using System.ComponentModel;

using Common.Controls;

namespace PassportMeetReservator.Controls
{
    public class ReserverInfoView : GroupBox
    {
        [Category("Inputs")]
        public NamedInput UrlInput { get; set; }
    }
}
