namespace PassportMeetReservator.Data.Platforms
{
    public class PoznanPlatformInfo : PlatformApiInfo
    {
        public override string Name => "Poznan Gov Ua";
        public override string Token => "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb21wYW55TmFtZSI6InV3cG96bmFuIiwiY29tcGFueUlkIjoiMSIsIm5iZiI6MTYwMTgwNzgyOSwiZXhwIjoyNTM0MDIyOTcyMDAsImlzcyI6IlFNU1dlYlJlc2VydmF0aW9uLkFQSSIsImF1ZCI6IlFNU1dlYlJlc2VydmF0aW9uLkNMSUVOVCJ9.IwDI0942FsfeN2Vm-0tFrg_3aKHU9dsouPpxRYl1OAw";
        public override string ApiUrl => "https://rejestracjapoznan.poznan.uw.gov.pl/api";
        public override string GetAvailableDatesApiMethod => "Slot/GetAvailableDaysForOperation";
        public override string GeneralErrorMessage => "General error.";
        public override CityPlatformInfo[] CityPlatforms { get; protected set; }
        public override PlatformCssInfo CssInfo { get; protected set; }

        public PoznanPlatformInfo() 
        {
            CityPlatforms = new CityPlatformInfo[]
            {
                new PoznanInfo(), new KaliszuInfo(), new KoninInfo(),
                new LesznieInfo(), new PileInfo()
            };

            CssInfo = new PoznanPlatformCssInfo();
        }
        protected class PoznanPlatformCssInfo : PlatformCssInfo
        {
            public override string StepCircleColor => "rgb(87, 133, 226)";
        }
    }

    public class BezkolejkiPlatformInfo : PlatformApiInfo
    {
        public override string Name => "Bezkolejki";
        public override string Token => "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb21wYW55TmFtZSI6InN1dyIsImNvbXBhbnlJZCI6IjgiLCJuYmYiOjE2MDQ4NTAxMDIsImV4cCI6MjUzNDAyMjk3MjAwLCJpc3MiOiJRTVNXZWJSZXNlcnZhdGlvbi5BUEkiLCJhdWQiOiJRTVNXZWJSZXNlcnZhdGlvbi5DTElFTlQifQ.MhQ2c-uEtGAr5kHPWP-H2ok6qcsLyd7ZnvmfJTNtemQ";
        public override string ApiUrl => "https://bezkolejki.eu/api/";
        public override string GetAvailableDatesApiMethod => "Slot/GetAvailableDaysForOperation";
        public override string GeneralErrorMessage => "General error.";
        public override CityPlatformInfo[] CityPlatforms { get; protected set; }
        public override PlatformCssInfo CssInfo { get; protected set; }

        public BezkolejkiPlatformInfo()
        {
            CityPlatforms = new CityPlatformInfo[]
            {
                new BezkolejkiInfo()
            };

            CssInfo = new BezkolejkiPlatformCssInfo();
        }

        protected class BezkolejkiPlatformCssInfo : PlatformCssInfo
        {
            public override string StepCircleColor => "rgb(208, 69, 69)";
        }
    }
}
