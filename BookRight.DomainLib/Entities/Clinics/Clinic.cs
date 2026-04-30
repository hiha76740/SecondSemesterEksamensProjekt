using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Clinics;

public class Clinic
{
    public string Name { get; private set; }

    public int TreatmentRoomLimit { get; private set; }

    public OpeningHours OpeningHours { get; private set; }

    // TODO: Tilføj Address Property

    public static Clinic Create(string name, int treatmentRoomLimit, OpeningHours openingHours)
    {
        var clinic = new Clinic(name, treatmentRoomLimit, openingHours);

        return clinic;
    }

    public void ChangeOpeningHours(OpeningHours newOpeningHours)
    {
        EnsureValidTime(newOpeningHours.Open);

        OpeningHours = newOpeningHours;
    }

    // TODO: Lav ChangeAddress metode


    private Clinic(string name, int treatmentRoomLimit, OpeningHours openingHours)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Clinic must have a name");
        if (treatmentRoomLimit < 1)
            throw new DomainException("Clinic must have atleast 1 treatment room");
        
        EnsureValidTime(openingHours.Open);


        Name = name;
        TreatmentRoomLimit = treatmentRoomLimit;
        OpeningHours = openingHours;
    }

    private void EnsureValidTime(DateTime openingTime)
    {
        if (openingTime < DateTime.Now)
            throw new DomainException("Opening time must be in the future");
    }
}
