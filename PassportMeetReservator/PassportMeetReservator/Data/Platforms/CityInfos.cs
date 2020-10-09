namespace PassportMeetReservator.Data.Platforms
{
    public class PoznanInfo : CityPlatformInfo
    {
        public PoznanInfo() : base("Poznan", "https://rejestracjapoznan.poznan.uw.gov.pl/")
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

    public class SubPoznanaInfo : CityPlatformInfo
    {
        public SubPoznanaInfo(string name, string baseUrl) : base(name, baseUrl)
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
        public KaliszuInfo() : base("Kaliszu", "https://rejestracjakalisz.poznan.uw.gov.pl/") { }
    }

    public class KoninInfo : SubPoznanaInfo
    {
        public KoninInfo() : base("Konin", "https://rejestracjakonin.poznan.uw.gov.pl/") { }
    }

    public class LesznieInfo : SubPoznanaInfo
    {
        public LesznieInfo() : base("Lesznie", "https://rejestracjaleszno.poznan.uw.gov.pl/") { }
    }

    public class PileInfo : SubPoznanaInfo
    {
        public PileInfo() : base("Pile", "https://rejestracjapila.poznan.uw.gov.pl/") { }
    }
}
