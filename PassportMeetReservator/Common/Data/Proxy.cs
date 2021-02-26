namespace Common.Data
{
    public class Proxy
    {
        public string Address { get; private set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsInUse { get; set; }
        public bool IsBlocked { get; set; }

        public Proxy(string address)
        {
            Address = address;
        }
    }
}
