using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Clinics;

/// <summary>
/// Represents a clinic with a name, a limit on treatment rooms, and defined opening hours.
/// </summary>
/// <remarks>The Clinic class provides core information about a clinic's identity and operational constraints. Use
/// the Create method to instantiate a Clinic with valid parameters. The class enforces that each clinic must have a
/// name, at least one treatment room, and valid opening hours. Thread safety is not guaranteed for instance
/// members.</remarks>
public class Clinic : AggregateRoot
{
    public string Name { get; private set; }

    public int TreatmentRoomLimit { get; private set; }

    public OpeningHours OpeningHours { get; private set; }

    public Address Address { get; private set; }

    /// <summary>
    /// Creates a new instance of the Clinic class with the specified name, treatment room limit, and opening hours.
    /// </summary>
    /// <param name="name">The name of the clinic. Cannot be null or empty.</param>
    /// <param name="treatmentRoomLimit">The maximum number of treatment rooms available in the clinic. Must be greater than zero.</param>
    /// <param name="openingHours">The opening hours for the clinic. Specifies when the clinic is available for appointments.</param>
    /// <returns>A Clinic instance initialized with the provided name, treatment room limit, and opening hours.</returns>
    public static Clinic Create(string name, int treatmentRoomLimit, OpeningHours openingHours, Address address)
    {
        var clinic = new Clinic(name, treatmentRoomLimit, openingHours, address);

        return clinic;
    }

    /// <summary>
    /// Updates the opening hours for the current instance.
    /// </summary>
    /// <param name="newOpeningHours">The new opening hours to apply. Must specify a valid opening time.</param>
    public void ChangeOpeningHours(OpeningHours newOpeningHours)
    {
        EnsureValidTime(newOpeningHours.Open);

        OpeningHours = newOpeningHours;
    }

    public void ChangeAddress(Address newAddress)
    {
        if (Address == newAddress)
            throw new DomainException("New and old address can't be the same");

        Address = newAddress;
    }


    private Clinic(string name, int treatmentRoomLimit, OpeningHours openingHours, Address address)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Clinic must have a name");
        if (treatmentRoomLimit < 1)
            throw new DomainException("Clinic must have atleast 1 treatment room");

        EnsureValidTime(openingHours.Open);

        Id = Guid.NewGuid();
        Name = name;
        TreatmentRoomLimit = treatmentRoomLimit;
        OpeningHours = openingHours;
        Address = address;
    }

    /// <summary>
    /// Validates that the specified opening time is in the future.
    /// </summary>
    /// <param name="openingTime">The date and time to validate. Must represent a future point in time.</param>
    /// <exception cref="DomainException">Thrown if openingTime is earlier than the current date and time.</exception>
    private static void EnsureValidTime(DateTime openingTime)
    {
        if (openingTime < DateTime.Now)
            throw new DomainException("Opening time must be in the future");
    }
}
