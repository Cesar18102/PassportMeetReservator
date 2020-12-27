using System;

using Common.Data.Dto;

namespace Common.Data.CustomEventArgs
{
    public class SlotBlockedEventArgs : EventArgs
    {
        public TimeCheckDto Slot { get; set; }
        public DateBlockDto Block { get; set; }
    }
}
