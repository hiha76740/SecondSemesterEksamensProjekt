using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Treatments;

public class Treatment : AggregateRoot
{
    public string Name { get; init; }
    public decimal Price { get; init; }
    public TimeSpan Duration { get; init; }
    public CertificationTypes CertificationRequired { get; init; }
    public int MaxParticipants { get; private set; }


    public static Treatment Create(string name, decimal price, TimeSpan duration, CertificationTypes certificationTypeRequired, int maxParticipants)
    {
        return new Treatment(name, price, duration, certificationTypeRequired, maxParticipants);
    }


    public void ChangeMaxParticipants(int newMaxParticipantsAmount)
    {
        if (newMaxParticipantsAmount < 1)
            throw new DomainException("Max participants amount can not be less than 1");

        MaxParticipants = newMaxParticipantsAmount;
    }


    private Treatment(string name, decimal price, TimeSpan duration, CertificationTypes certificationRequired, int maxParticipants)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name of treatment can not be empty");

        if (price < 1)
            throw new DomainException("Price can not be 0 or negative");

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Duration = duration;
        CertificationRequired = certificationRequired;
        MaxParticipants = maxParticipants;
    }
}
