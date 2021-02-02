using System;

namespace Common.Data.Platforms
{
    public abstract class CityPlatformInfo : IEquatable<CityPlatformInfo>
    {
        public abstract string Name { get; }
        public abstract string BaseUrl { get; }
        public abstract string Authority { get; }

        public abstract string Referer { get; }
        public abstract string AltApiUrl { get; }
        public abstract string AltBlockSlotApiMethod { get; }

        public abstract OperationInfo[] Operations { get; protected set; }
        public abstract PlatformCssInfo CssInfo { get; protected set; }

        public bool Equals(CityPlatformInfo other)
        {
            return this.Name == other.Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
