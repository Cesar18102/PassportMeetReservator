using System.Windows.Forms;
using System.ComponentModel;

namespace PassportMeetReservator.Controls
{
    public class ReserverInfoView : GroupBox
    {
        [Category("Inputs")]
        public NamedInput SurnameInput { get; set; }

        [Category("Inputs")]
        public NamedInput NameInput { get; set; }

        [Category("Inputs")]
        public NamedInput EmailInput { get; set; }

        [Category("Inputs")]
        public NamedInput UrlInput { get; set; }
    }
}
