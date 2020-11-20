namespace PassportMeetReservator.Data.Platforms
{
    public abstract class PlatformApiInfo
    {
        public abstract string Name { get; }
        public abstract string Token { get; }
        public abstract string ApiUrl { get; }

        public abstract string GetAvailableDatesApiMethod { get; }
        public abstract string GetAvailableSlotsForDateApiMethod { get; }

        public abstract string GeneralErrorMessage { get; }
        public abstract CityPlatformInfo[] CityPlatforms { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
