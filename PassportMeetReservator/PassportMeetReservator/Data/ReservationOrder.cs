namespace PassportMeetReservator.Data
{
    public class ReservationOrder
    {
        public string ReservationTypeText { get; set; }
        
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public bool Done { get; set; } = false;

        public override string ToString()
        {
            return $"Surname: {Surname}; Name: {Name}; Email: {Email}";
        }
    }
}
