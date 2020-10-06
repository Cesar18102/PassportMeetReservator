using Newtonsoft.Json;

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

        public string City { get; set; }
        public string CityUrl { get; set; }

        public string Operation { get; set; }
        public int OperationNumber { get; set; }

        public ReservationOrder(string surname, string name, string email, string city, string cityUrl, string operation, int operationNumber)
        {
            Surname = surname;
            Name = name;
            Email = email;

            City = city;
            CityUrl = cityUrl;

            Operation = operation;
            OperationNumber = operationNumber;
        }
    }
}
