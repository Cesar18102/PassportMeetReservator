namespace PassportMeetReservator.Data.Platforms
{
    public class PoznanPlatformCssInfo : PlatformCssInfo
    {
        public override string StepCircleColor => "rgb(87, 133, 226)";
    }

    public class PoznanInfo : CityPlatformInfo
    {
        public override string Name => "Poznan";
        public override string BaseUrl => "https://rejestracjapoznan.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjapoznan.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjapoznan.poznan.uw.gov.pl/";
        public override string AltApiUrl => "https://rejestracjapoznan.poznan.uw.gov.pl/api/";
        public override OperationInfo[] Operations { get; protected set; }
        public override PlatformCssInfo CssInfo { get; protected set; }

        public PoznanInfo()
        { 
            Operations = new OperationInfo[]
            {
                new OperationInfo("PASZPORTY - Składanie wniosków o paszport", 1, 0),
                new OperationInfo("PASZPORTY - Odbiór paszportu", 2, 1),
                new OperationInfo("CUDZOZIEMCY - Odbiór karty pobytu", 7, 2),
                new OperationInfo("CUDZOZIEMCY - Złożenie wniosku: pobyt czasowy / stały / rezydenta UE, wydanie / wymiana karty pobytu, świadczenie pieniężne dla posiadaczy Karty Polaka", 8, 3),
                new OperationInfo("CUDZOZIEMCY - Złożenie odcisków palców (do wniosków przesłanych pocztą / złożonych w delegaturach)", 9, 4),
                new OperationInfo("CUDZOZIEMCY - Uzyskanie stempla (pieczątki) w paszporcie (wyłącznie wnioski ze statusem pozytywna weryfikacja formalna)", 10, 5),
                new OperationInfo("Obywatelstwo polskie", 12, 6)
            };

            CssInfo = new PoznanPlatformCssInfo();
        }
    }

    public abstract class SubPoznanaInfo : CityPlatformInfo
    {
        public override OperationInfo[] Operations { get; protected set; }
        public override PlatformCssInfo CssInfo { get; protected set; }

        public SubPoznanaInfo()
        {
            Operations = new OperationInfo[]
            {
                new OperationInfo("PASZPORTY - Składanie wniosków o paszport", 1, 0),
                new OperationInfo("PASZPORTY - Odbiór paszportu", 2, 1),
                new OperationInfo("CUDZOZIEMCY - Odbiór karty pobytu", 5, 2),
                new OperationInfo("CUDZOZIEMCY - Uzupełnienia / odciski", 7, 3)
            };

            CssInfo = new PoznanPlatformCssInfo();
        }
    }

    public class KaliszuInfo : SubPoznanaInfo
    {
        public override string Name => "Kaliszu";
        public override string BaseUrl => "https://rejestracjakalisz.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjakalisz.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjakalisz.poznan.uw.gov.pl/";
        public override string AltApiUrl => "https://rejestracjakalisz.poznan.uw.gov.pl/api/";
    }

    public class KoninInfo : SubPoznanaInfo
    {
        public override string Name => "Konin";
        public override string BaseUrl => "https://rejestracjakonin.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjakonin.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjakonin.poznan.uw.gov.pl/";
        public override string AltApiUrl => "https://rejestracjakonin.poznan.uw.gov.pl/api/";
    }

    public class LesznieInfo : SubPoznanaInfo
    {
        public override string Name => "Lesznie";
        public override string BaseUrl => "https://rejestracjaleszno.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjaleszno.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjaleszno.poznan.uw.gov.pl/";
        public override string AltApiUrl => "https://rejestracjaleszno.poznan.uw.gov.pl/api/";
    }

    public class PileInfo : SubPoznanaInfo
    {
        public override string Name => "Pile";
        public override string BaseUrl => "https://rejestracjapila.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjapila.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjapila.poznan.uw.gov.pl/";
        public override string AltApiUrl => "https://rejestracjapila.poznan.uw.gov.pl/api/";
    }

    public class KatowiceInfo : CityPlatformInfo
    {
        public override string Name => "Katowice";
        public override string BaseUrl => "https://bezkolejki.eu/suw/";
        public override string Authority => "bezkolejki.eu";
        public override string Referer => "https://bezkolejki.eu/suw/";
        public override string AltApiUrl => null;
        public override OperationInfo[] Operations { get; protected set; }
        public override PlatformCssInfo CssInfo { get; protected set; }


        public KatowiceInfo()
        {
            Operations = new OperationInfo[]
            {
                new OperationInfo("Paszporty - Złożenie wniosku paszportowego", 79, 0),
                new OperationInfo("Paszporty - Odbiór paszportu", 99, 1),
                new OperationInfo("Zezwolenia na pracę (obsługa pracodawców) - Zezwolenia na pracę (obsługa pracodawców)", 98, 2),
                new OperationInfo("Pobyt - Odbiór karty pobytu lub decyzji", 95, 3),
                new OperationInfo("Rejestracja Obywateli UE i członków ich rodzin - Składanie wniosków", 94, 4),
                new OperationInfo("Rejestracja Obywateli UE i członków ich rodzin - Odbiór dokumentów", 93, 5),
                new OperationInfo("Zaproszenia - Składanie wniosków", 92, 6),
                new OperationInfo("Zaproszenia - Odbiór zaproszenia", 91, 7),
                new OperationInfo("Świadczenia pieniężne dla posiadaczy Karty Polaka - Składanie wniosków", 6710, 8),
                new OperationInfo("Świadczenia pieniężne dla posiadaczy Karty Polaka - Odbiór decyzji", 6708, 9),
                new OperationInfo("Inne - Potwierdzanie profilu zaufanego", 6709, 10),
                new OperationInfo("Pobyt - Punkt informacyjno-doradczy dla cudzoziemców", 6697, 11)
            };

            CssInfo = new KatowiceCssInfo();
        }

        protected class KatowiceCssInfo : PlatformCssInfo
        {
            public override string StepCircleColor => "rgb(208, 69, 69)";
        }
    }

    public class GorzowInfo : CityPlatformInfo
    {
        public override string Name => "Gorzow";
        public override string BaseUrl => "https://bezkolejki.eu/luw-gorzow";
        public override string Authority => "bezkolejki.eu";
        public override string Referer => "https://bezkolejki.eu/luw-gorzow";
        public override string AltApiUrl => null;
        public override OperationInfo[] Operations { get; protected set; }
        public override PlatformCssInfo CssInfo { get; protected set; }

        public GorzowInfo()
        {
            Operations = new OperationInfo[]
            {
                new OperationInfo("Przyjmowanie wniosków o pobyt w Polsce", 7063, 0),
                new OperationInfo("Sprawy cudzoziemców- stemple i odciski palców", 7064, 1),
                new OperationInfo("Odbiór kart pobytu", 7066, 2),
                new OperationInfo("Sprawy dotyczące obywatelstwa polskiego", 7071, 3),
                new OperationInfo("Przyjmowanie wniosków o paszport, wniosków ob. UE, zaproszeń", 7073, 4),
                new OperationInfo("Odbiór paszportu, dokumentów ob. UE, zaproszeń", 7074, 5)
            };

            CssInfo = new GorzowCssInfo();
        }

        protected class GorzowCssInfo : PlatformCssInfo
        {
            public override string StepCircleColor => "rgb(75, 164, 236)";
        }
    }
}
