using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class ReservationOrder
    {
        public string ReservationTypeText { get; set; }
        
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public bool Done { get; set; } = false;

        [JsonIgnore]
        public bool Doing { get; set; } = false;

        public override string ToString()
        {
            return $"Surname: {Surname}; Name: {Name}; Email: {Email}";
        }
    }
}
