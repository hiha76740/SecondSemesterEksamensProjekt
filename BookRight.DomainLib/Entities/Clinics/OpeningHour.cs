using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Clinics;

public class OpeningHour: Entity
{
    public Weekdays Weekday { get; init; }
    public TimeOnly? OpeningTime { get; private set; }
    public TimeOnly? CloseingTime { get; private set; }
    public bool IsClosed { get; private set; }


    internal void ChangeOpeningHourTime(TimeOnly? openingTime, TimeOnly? closingTime, bool isClosed)
    {
        ApplyClosedState(isClosed, openingTime, closingTime);
    }

    internal OpeningHour(Weekdays weekday, TimeOnly? openingTime, TimeOnly? closingTime, bool isClosed)
    {
        Id = Guid.NewGuid();
        Weekday = weekday;

        ApplyClosedState(isClosed,openingTime, closingTime);        
    }

    private void ApplyClosedState(bool isClosed, TimeOnly? openingTime, TimeOnly? closingTime)
    {
        if (isClosed == true)
        {
            OpeningTime = null;
            CloseingTime = null;
            IsClosed = isClosed;
        }
        else
        {
            OpeningTime = openingTime;
            CloseingTime = closingTime;
            IsClosed = isClosed;
        }
    }
}
