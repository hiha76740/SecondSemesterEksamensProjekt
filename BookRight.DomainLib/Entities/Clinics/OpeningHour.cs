using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Clinics;

/// <summary>
/// Represents the opening and closing times for a single weekday, with support for an explicit closed state.
/// </summary>
/// <remarks>When IsClosed is true, OpeningTime and ClosingTime are null. Instances are initialized and updated
/// via the internal API; a private parameterless constructor is present for EF materialization.</remarks>
public class OpeningHour: Entity
{
    public WeekDays WeekDay { get; init; }
    public TimeOnly? OpeningTime { get; private set; }
    public TimeOnly? ClosingTime { get; private set; }
    public bool IsClosed { get; private set; }

    /// <summary>
    /// Sets the opening and closing times and the closed state.
    /// </summary>
    /// <remarks>Delegates the update to ApplyClosedState(bool, TimeOnly?, TimeOnly?).</remarks>
    /// <param name="openingTime">The opening time to apply, or null if unspecified.</param>
    /// <param name="closingTime">The closing time to apply, or null if unspecified.</param>
    /// <param name="isClosed">True to mark as closed; otherwise false.</param>
    internal void ChangeOpeningHourTime(TimeOnly? openingTime, TimeOnly? closingTime, bool isClosed)
    {
        ApplyClosedState(isClosed, openingTime, closingTime);
    }

    /// <summary>
    /// Initializes a new instance of OpeningHour for the specified weekday.
    /// </summary>
    /// <remarks>Generates a new Id and applies the closed/open state using ApplyClosedState.</remarks>
    /// <param name="weekDay">The weekday the opening hours apply to.</param>
    /// <param name="openingTime">Optional opening time; null when the day is closed or a time is not applicable.</param>
    /// <param name="closingTime">Optional closing time; null when the day is closed or a time is not applicable.</param>
    /// <param name="isClosed">True if the day is closed; when true, openingTime and closingTime are ignored.</param>
    internal OpeningHour(WeekDays weekDay, TimeOnly? openingTime, TimeOnly? closingTime, bool isClosed)
    {
        Id = Guid.NewGuid();
        WeekDay = weekDay;

        ApplyClosedState(isClosed,openingTime, closingTime);        
    }

    /// <summary>
    /// Sets OpeningTime, ClosingTime, and IsClosed; clears times when isClosed is true.
    /// </summary>
    /// <remarks>When isClosed is true, OpeningTime and ClosingTime are set to null to ensure a consistent
    /// closed state.</remarks>
    /// <param name="isClosed">true to mark the entity as closed and clear the opening and closing times; false to apply the provided times.</param>
    /// <param name="openingTime">Nullable opening time to assign when isClosed is false; ignored when isClosed is true.</param>
    /// <param name="closingTime">Nullable closing time to assign when isClosed is false; ignored when isClosed is true.</param>
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

    private OpeningHour() { }
}
