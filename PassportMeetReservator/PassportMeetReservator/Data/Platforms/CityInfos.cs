namespace PassportMeetReservator.Data.Platforms
{ 
    public class PoznanInfo : CityPlatformInfo
    {
        public override string Name => "Poznan";
        public override string BaseUrl => "https://rejestracjapoznan.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjapoznan.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjapoznan.poznan.uw.gov.pl/";
        public override OperationInfo[] Operations { get; protected set; }

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
        }
    }

    public abstract class SubPoznanaInfo : CityPlatformInfo
    {
        public override OperationInfo[] Operations { get; protected set; }

        public SubPoznanaInfo()
        {
            Operations = new OperationInfo[]
            {
                new OperationInfo("PASZPORTY - Składanie wniosków o paszport", 1, 0),
                new OperationInfo("PASZPORTY - Odbiór paszportu", 2, 1),
                new OperationInfo("CUDZOZIEMCY - Odbiór karty pobytu", 5, 2),
                new OperationInfo("CUDZOZIEMCY - Uzupełnienia / odciski", 7, 3)
            };
        }
    }

    public class KaliszuInfo : SubPoznanaInfo
    {
        public override string Name => "Kaliszu";
        public override string BaseUrl => "https://rejestracjakalisz.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjakalisz.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjakalisz.poznan.uw.gov.pl/";
    }

    public class KoninInfo : SubPoznanaInfo
    {
        public override string Name => "Konin";
        public override string BaseUrl => "https://rejestracjakonin.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjakonin.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjakonin.poznan.uw.gov.pl/";
    }

    public class LesznieInfo : SubPoznanaInfo
    {
        public override string Name => "Lesznie";
        public override string BaseUrl => "https://rejestracjaleszno.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjaleszno.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjaleszno.poznan.uw.gov.pl/";
    }

    public class PileInfo : SubPoznanaInfo
    {
        public override string Name => "Pile";
        public override string BaseUrl => "https://rejestracjapila.poznan.uw.gov.pl/";
        public override string Authority => "rejestracjapila.poznan.uw.gov.pl";
        public override string Referer => "https://rejestracjapila.poznan.uw.gov.pl/";
    }

    public class BezkolejkiInfo : CityPlatformInfo
    {
        public override string Name => "Bezkolejki";
        public override string BaseUrl => "https://bezkolejki.eu/suw/";
        public override string Authority => "bezkolejki.eu";
        public override string Referer => "https://bezkolejki.eu/suw/";
        public override OperationInfo[] Operations { get; protected set; }

        public BezkolejkiInfo()
        {
            Operations = new OperationInfo[]
            {
                new OperationInfo("Paszporty - Złożenie wniosku paszportowego", 79, 0)
            };
        }
    }
}
