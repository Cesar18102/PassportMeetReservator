namespace PassportMeetReservator.Data.Platforms
{
    public class PoznanInfo : CityPlatformInfo
    {
        public PoznanInfo() : base("Poznan", "https://rejestracjapoznan.poznan.uw.gov.pl/")
        {
            Operations = new string[]
            {
                "PASZPORTY - Składanie wniosków o paszport",
                "PASZPORTY - Odbiór paszportu",
                "CUDZOZIEMCY - Odbiór karty pobytu",
                "CUDZOZIEMCY - Złożenie wniosku: pobyt czasowy / stały / rezydenta UE, wydanie / wymiana karty pobytu, świadczenie pieniężne dla posiadaczy Karty Polaka",
                "CUDZOZIEMCY - Złożenie odcisków palców (do wniosków przesłanych pocztą / złożonych w delegaturach)",
                "CUDZOZIEMCY - Uzyskanie stempla(pieczątki) w paszporcie(wyłącznie wnioski ze statusem pozytywna weryfikacja formalna)",
                "Obywatelstwo polskie"
            };
        }
    }

    public class SubPoznanaInfo : CityPlatformInfo
    {
        public SubPoznanaInfo(string name, string baseUrl) : base(name, baseUrl)
        {
            Operations = new string[]
            {
                "PASZPORTY - Składanie wniosków o paszport",
                "PASZPORTY - Odbiór paszportu",
                "CUDZOZIEMCY - Odbiór karty pobytu",
                "CUDZOZIEMCY - Uzupełnienia / odciski"
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
