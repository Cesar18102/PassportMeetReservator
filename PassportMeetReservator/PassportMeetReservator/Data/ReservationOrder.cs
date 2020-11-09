using Newtonsoft.Json;
using PassportMeetReservator.Data.Platforms;

namespace PassportMeetReservator.Data
{
    public class ReservationOrder
    {
        [JsonIgnore]
        public bool Doing { get; set; }

        public bool Done { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Platform { get; set; }

        public string City { get; set; }
        public string CityUrl { get; set; }

        public OperationInfo Operation { get; set; }

        public ReservationOrder(string surname, string name, string email, string platform, string city, string cityUrl, OperationInfo operation)
        {
            Surname = surname;
            Name = name;
            Email = email;

            Platform = platform;
            City = city;
            CityUrl = cityUrl;

            Operation = operation;
        }
    }
}
