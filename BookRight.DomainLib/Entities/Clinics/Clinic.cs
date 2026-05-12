using BookRight.DomainLib.Enums;
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
    public string Name { get; private set; } = null!;
    public int TreatmentRoomLimit { get; private set; }

    public Address Address { get; private set; } = null!;

    private readonly List<OpeningHour> _openingHours = new();
    public IReadOnlyCollection<OpeningHour> OpeningHours => _openingHours.AsReadOnly();


    public static Clinic Create(string name, int treatmentRoomLimit, List<OpeningHourInput> openingHoursInput, Address address)
    {
        List<OpeningHour> openingHours = new List<OpeningHour>();
        
        foreach (var openingHour in openingHoursInput)
        {
            var oh = CreateOpeningHour(openingHour.Weekday, openingHour.Open, openingHour.Close, openingHour.IsClosed);

            if (openingHours.Any(x => x.Weekday == openingHour.Weekday) == true)
                throw new DomainException("Opening hours can't have 2 of the same day");

            openingHours.Add(oh);
        }

        if (openingHours.Count != 7)
            throw new DomainException("Clinic must have opening hour for each day of the week");

        var clinic = new Clinic(name, treatmentRoomLimit, openingHours, address);

        return clinic;
    }

    public static OpeningHour CreateOpeningHour(Weekdays weekdays, TimeOnly? openingTime, TimeOnly? closeingTime, bool isClosed)
    {
        return new OpeningHour(weekdays, openingTime, closeingTime, isClosed);
    }


    public void ChangeOpeningHour(Guid OpeningHourId,OpeningHourInput openingHourInput)
    {
        var exsist = _openingHours.FirstOrDefault(oh => oh.Id == OpeningHourId);

        if (exsist == null)
            throw new NotFoundException("OpeningHour was not found");

        exsist.ChangeOpeningHourTime(openingHourInput.Open, openingHourInput.Close, openingHourInput.IsClosed); 
    }

    public void ChangeAddress(Address newAddress)
    {
        if (Address == newAddress)
            throw new DomainException("New and old address can't be the same");

        Address = newAddress;
    }

    public void ChangeTreatmentRoomLimit(int newTreatmentRoomLimit)
    {
        EnsureValidTreatmentRoomLimit(newTreatmentRoomLimit);

        TreatmentRoomLimit = newTreatmentRoomLimit;
    }

   


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

    private static void EnsureValidTreatmentRoomLimit(int treatmentRoomLimit)
    {
        if (treatmentRoomLimit < 1)
            throw new DomainException("Clinic must have atleast 1 treatment room");
    }

    // EF Constructor
    private Clinic() { }
}
