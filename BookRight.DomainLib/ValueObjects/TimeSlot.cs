using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects
{
    public record TimeSlot
    {
        public DateTime From { get; init; }
        public DateTime To { get; init; }

        private TimeSlot()
        {
            // Required by EF Core
        }

        public TimeSlot(DateTime from, DateTime to)
        {
            if (to <= from)
            {
                throw new DomainException("Sluttidspunkt skal være efter starttidspunkt.");
            }

            From = from;
            To = to;
        }

        public TimeSpan Duration => To - From;

        public bool OverlapsWith(TimeSlot other)
        {
            if (other == null)
            {
                throw new DomainException("Tidsinterval er påkrævet.");
            }

            return From < other.To && other.From < To;
        }
    }
}
