namespace PassportMeetReservator.Data
{
    public class ReservationOrder
    {
        public bool Doing { get; set; }
        public bool Done { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Operation { get; set; }

        public ReservationOrder(string surname, string name, string email, string operation)
        {
            Surname = surname;
            Name = name;
            Email = email;
            Operation = operation;
        }
    }
}
