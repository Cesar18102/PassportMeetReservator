namespace PassportMeetReservator.Data.Platforms
{
    public abstract class CityPlatformInfo
    {
        public abstract string Name { get; }

        public abstract string BaseUrl { get; }

        public abstract string Authority { get; }

        public abstract string Referer { get; }

        public abstract OperationInfo[] Operations { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
