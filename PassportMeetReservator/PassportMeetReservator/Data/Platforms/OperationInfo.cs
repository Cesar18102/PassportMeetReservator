namespace PassportMeetReservator.Data.Platforms
{
    public class OperationInfo
    {
        public string Name { get; private set; }
        public int Number { get; private set; }
        public int Position { get; private set; }

        public OperationInfo(string name, int number, int position)
        {
            Name = name;
            Number = number;
            Position = position;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
