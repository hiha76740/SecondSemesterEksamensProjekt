using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects
{
    /// <summary>
    /// Value Object representing a time interval.
    /// </summary>
    public record TimeSlot
    {
        public DateTime From { get; init; }
        public DateTime To { get; init; }

        public TimeSlot(DateTime from, DateTime to)
        {
            if (to <= from)
                throw new DomainException("End time must be after start time.");

            From = from;
            To = to;
        }

        public TimeSpan Duration => To - From;

        internal bool OverlapsWith(TimeSlot other)
            => From < other.To && other.From < To;

        // Parameterless constructor for EF Core
        private TimeSlot() { }
    }
}
