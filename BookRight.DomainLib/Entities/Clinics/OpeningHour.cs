using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Clinics;

public class OpeningHour: Entity
{
    public WeekDays WeekDay { get; init; }
    public TimeOnly? OpeningTime { get; private set; }
    public TimeOnly? ClosingTime { get; private set; }
    public bool IsClosed { get; private set; }


    internal void ChangeOpeningHourTime(TimeOnly? openingTime, TimeOnly? closingTime, bool isClosed)
    {
        ApplyClosedState(isClosed, openingTime, closingTime);
    }

    internal OpeningHour(WeekDays weekDay, TimeOnly? openingTime, TimeOnly? closingTime, bool isClosed)
    {
        Id = Guid.NewGuid();
        WeekDay = weekDay;

        ApplyClosedState(isClosed,openingTime, closingTime);        
    }

    private void ApplyClosedState(bool isClosed, TimeOnly? openingTime, TimeOnly? closingTime)
    {
        if (isClosed == true)
        {
            OpeningTime = null;
            ClosingTime = null;
            IsClosed = isClosed;
        }
        else
        {
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            IsClosed = isClosed;
        }
    }

    // EF Constructor
    private OpeningHour() { }
}
