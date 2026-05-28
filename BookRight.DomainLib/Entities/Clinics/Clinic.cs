using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Clinics;

/// <summary>
/// Represents a clinic aggregate root that encapsulates identity, name, address, daily opening hours, and the number of
/// treatment rooms. Instances are created via the Create factory, which requires exactly one opening hour per weekday
/// and validates the treatment room limit.
/// </summary>
/// <remarks>Creation and mutation operations enforce domain rules: name must not be empty; treatment room limit
/// must be at least 1; opening hours must include exactly one entry for each day of the week and must not contain
/// duplicate days. Methods allow changing an individual opening hour, the clinic address, and the treatment room limit
/// with validation. A parameterless constructor exists for ORM use.</remarks>
public class Clinic : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public int TreatmentRoomLimit { get; private set; }

    public Address Address { get; private set; } = null!;

    private readonly List<OpeningHour> _openingHours = new();
    public IReadOnlyCollection<OpeningHour> OpeningHours => _openingHours.AsReadOnly();

    /// <summary>
    /// Creates a Clinic with the specified name, treatment room limit, opening hours, and address.
    /// </summary>
    /// <param name="name">Clinic name.</param>
    /// <param name="treatmentRoomLimit">Maximum number of treatment rooms.</param>
    /// <param name="openingHoursInput">Collection of opening hour inputs representing each weekday; must contain exactly one entry per weekday (7
    /// total).</param>
    /// <param name="address">Clinic address.</param>
    /// <returns>A new Clinic instance.</returns>
    /// <exception cref="DomainException">Thrown when openingHoursInput contains duplicate weekdays or does not provide an opening hour for each day of
    /// the week.</exception>

    public static Clinic Create(string name, int treatmentRoomLimit, List<OpeningHourInput> openingHoursInput, Address address)
    {
        List<OpeningHour> openingHours = new List<OpeningHour>();
        
        foreach (var openingHour in openingHoursInput)
        {
            var oh = CreateOpeningHour(openingHour.WeekDay, openingHour.Open, openingHour.Close, openingHour.IsClosed);

            if (openingHours.Any(x => x.WeekDay == openingHour.WeekDay) == true)
                throw new DomainException("Opening hours can't have 2 of the same day");

            openingHours.Add(oh);
        }

        if (openingHours.Count != 7)
            throw new DomainException("Clinic must have opening hour for each day of the week");

        var clinic = new Clinic(name, treatmentRoomLimit, openingHours, address);

        return clinic;
    }

    /// <summary>
    /// Creates an OpeningHour for the specified days with the given opening and closing times and closed state.
    /// </summary>
    /// <remarks>When isClosed is true, openingTime and closingTime are ignored. Null times are allowed and
    /// are preserved on the returned OpeningHour.</remarks>
    /// <param name="weekDays">Days of the week the opening hour applies to.</param>
    /// <param name="openingTime">Start time for the opening period, or null if not applicable.</param>
    /// <param name="closingTime">End time for the opening period, or null if not applicable.</param>
    /// <param name="isClosed">True if the period is closed for the specified days; otherwise false.</param>
    /// <returns>An OpeningHour representing the specified schedule.</returns>
    public static OpeningHour CreateOpeningHour(WeekDays weekDays, TimeOnly? openingTime, TimeOnly? closingTime, bool isClosed)
    {
        return new OpeningHour(weekDays, openingTime, closingTime, isClosed);
    }

    /// <summary>
    /// Updates the opening hour identified by OpeningHourId using values from openingHourInput.
    /// </summary>
    /// <remarks>Applies changes to the existing entity. Persist changes to the underlying store if
    /// required.</remarks>
    /// <param name="OpeningHourId">The identifier of the opening hour to update.</param>
    /// <param name="openingHourInput">An object containing the opening time, closing time, and a flag indicating whether the slot is closed.</param>
    /// <exception cref="NotFoundException">Thrown when no opening hour exists for the specified identifier.</exception>
    public void ChangeOpeningHour(Guid OpeningHourId,OpeningHourInput openingHourInput)
    {
        var exsist = _openingHours.FirstOrDefault(oh => oh.Id == OpeningHourId);

        if (exsist == null)
            throw new NotFoundException("OpeningHour was not found");

        exsist.ChangeOpeningHourTime(openingHourInput.Open, openingHourInput.Close, openingHourInput.IsClosed); 
    }

    /// <summary>
    /// Updates the address to the specified value.
    /// </summary>
    /// <param name="newAddress">The new address to assign; must not be equal to the current Address.</param>
    /// <exception cref="DomainException">Thrown when the specified address is equal to the current Address.</exception>
    public void ChangeAddress(Address newAddress)
    {
        if (Address == newAddress)
            throw new DomainException("New and old address can't be the same");

        Address = newAddress;
    }

    /// <summary>
    /// Updates the TreatmentRoomLimit to the specified value after validating it.
    /// </summary>
    /// <remarks>Validation is performed by EnsureValidTreatmentRoomLimit; an exception will be thrown if the
    /// value is invalid.</remarks>
    /// <param name="newTreatmentRoomLimit">The new treatment room limit to apply; validated before assignment.</param>
    public void ChangeTreatmentRoomLimit(int newTreatmentRoomLimit)
    {
        EnsureValidTreatmentRoomLimit(newTreatmentRoomLimit);

        TreatmentRoomLimit = newTreatmentRoomLimit;
    }

   

    /// <summary>
    /// Creates a Clinic with the specified name, treatment room limit, opening hours, and address.
    /// </summary>
    /// <remarks>Assigns a new Id and stores the provided values after validation.</remarks>
    /// <param name="name">Clinic name; must not be null or empty.</param>
    /// <param name="treatmentRoomLimit">Maximum number of treatment rooms; value is validated.</param>
    /// <param name="openingHours">List of opening hours for the clinic.</param>
    /// <param name="address">Clinic address.</param>
    /// <exception cref="DomainException">Thrown when name is null or empty, or when treatmentRoomLimit is invalid.</exception>
    private Clinic(string name, int treatmentRoomLimit, List<OpeningHour> openingHours, Address address)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Clinic must have a name");
        EnsureValidTreatmentRoomLimit(treatmentRoomLimit);

        Id = Guid.NewGuid();
        Name = name;
        TreatmentRoomLimit = treatmentRoomLimit;
        _openingHours = openingHours;
        Address = address;
    }

    /// <summary>
    /// Validates that the treatment room limit is at least 1.
    /// </summary>
    /// <remarks>Enforces the domain invariant that a clinic must have at least one treatment room.</remarks>
    /// <param name="treatmentRoomLimit">Number of treatment rooms to validate; must be at least 1.</param>
    /// <exception cref="DomainException">Thrown when treatmentRoomLimit is less than 1.</exception>
    private static void EnsureValidTreatmentRoomLimit(int treatmentRoomLimit)
    {
        if (treatmentRoomLimit < 1)
            throw new DomainException("Clinic must have atleast 1 treatment room");
    }

    private Clinic() { }
}
