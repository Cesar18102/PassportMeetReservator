using System;

using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class ReservationOrder : IEquatable<ReservationOrder>
    {
        public string ReservationTypeText { get; set; }
        
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public bool Done { get; set; } = false;

        [JsonIgnore]
        public bool Doing { get; set; } = false;

        public bool Equals(ReservationOrder other)
        {
            return Surname == other.Surname && Name == other.Name && Email == other.Email && 
                   ReservationTypeText == other.ReservationTypeText;
        }

        public override string ToString()
        {
            return $"Surname: {Surname}; Name: {Name}; Email: {Email}";
        }
    }
}
