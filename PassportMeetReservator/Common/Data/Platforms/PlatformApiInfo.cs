using System;

namespace Common.Data.Platforms
{
    public abstract class PlatformApiInfo : IEquatable<PlatformApiInfo>
    {
        public abstract int Id { get; }

        public abstract string Name { get; }
        public abstract string Token { get; }
        public abstract string ApiUrl { get; }

        public abstract string GetAvailableDatesApiMethod { get; }
        public abstract string GetAvailableSlotsForDateApiMethod { get; }
        public abstract string BlockSlotApiMethod { get; }

        public abstract string GeneralErrorMessage { get; }
        public abstract CityPlatformInfo[] CityPlatforms { get; protected set; }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(PlatformApiInfo other)
        {
            return this.Id == other.Id;
        }
    }
}
