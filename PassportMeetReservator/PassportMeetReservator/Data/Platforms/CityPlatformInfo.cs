namespace PassportMeetReservator.Data.Platforms
{
    public class CityPlatformInfo
    {
        public string Name { get; protected set; }
        public string BaseUrl { get; protected set; }
        public OperationInfo[] Operations { get; protected set; }

        public CityPlatformInfo(string name, string baseUrl)
        {
            Name = name;
            BaseUrl = baseUrl;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
