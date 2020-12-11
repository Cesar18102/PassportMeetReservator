using System;
using System.Drawing;

namespace PassportMeetReservator.Extensions
{
    public static class ImageExtensions
    {
        public static bool TrySave(this Bitmap bitmap, string path)
        {
            try { bitmap.Save(path); return true; }
            catch(Exception ex) { return false; }
        }
    }
}
