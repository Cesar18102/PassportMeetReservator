using System;

namespace Common.Data
{
    public class Proxy
    {
        public event EventHandler<EventArgs> IsInUseChanged;

        private bool isInUse;

        public string Address { get; private set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsInUse
        {
            get => isInUse;
            set
            {
                if (isInUse && !value)
                    LastUsed = DateTime.Now;

                bool oldValue = isInUse;

                isInUse = value;

                if (oldValue != isInUse)
                    IsInUseChanged?.Invoke(this, new EventArgs());
            }
        }

        public bool IsInUseByBrowser { get; set; }
        public bool IsBlocked { get; set; }

        public DateTime? LastUsed { get; set; }

        public Proxy(string address)
        {
            Address = address;
        }
    }
}
