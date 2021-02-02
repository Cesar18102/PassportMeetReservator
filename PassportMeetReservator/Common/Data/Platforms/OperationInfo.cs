using System;

namespace Common.Data.Platforms
{
    public class OperationInfo : IEquatable<OperationInfo>
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

        public bool Equals(OperationInfo other)
        {
            return this.Number == other.Number;
        }
    }
}
