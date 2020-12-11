using System.Drawing;
using System.Windows.Forms;

namespace PassportMeetReservator.Extensions
{
    public static class FormExtensions
    {
        public static Bitmap Snapshot(this Form form, Control control)
        {
            Point location = new Point(
                form.Location.X + control.Location.X,
                form.Location.Y + control.Location.Y
            );

            Bitmap bitmap = new Bitmap(control.Width, control.Height);

            using (Graphics canvas = Graphics.FromImage(bitmap))
                canvas.CopyFromScreen(location, new Point(0, 0), control.Size);

            return bitmap;
        }
    }
}
