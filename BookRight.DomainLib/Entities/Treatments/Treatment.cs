using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Treatments;

/// <summary>
/// Represents a treatment offered by the domain, including its name, price, duration, required certification, and
/// participant limit.
/// </summary>
/// <remarks>Instances are created via Create; an Id is generated automatically. The constructor and
/// ChangeMaxParticipants validate inputs and throw DomainException for invalid values (name must be non-empty, price
/// must be at least 1, and max participants must be at least 1).</remarks>
public class Treatment : AggregateRoot
{
    public string Name { get; init; }
    public decimal Price { get; init; }
    public TimeSpan Duration { get; init; }
    public CertificationTypes CertificationRequired { get; init; }
    public int MaxParticipants { get; private set; }

    /// <summary>
    /// Creates a new Treatment with the specified name, price, duration, required certification, and maximum
    /// participants.
    /// </summary>
    /// <param name="name">Name of the treatment.</param>
    /// <param name="price">Price of the treatment.</param>
    /// <param name="duration">Duration of the treatment.</param>
    /// <param name="certificationTypeRequired">Required certification type to perform the treatment.</param>
    /// <param name="maxParticipants">Maximum number of participants allowed.</param>
    /// <returns>A Treatment instance initialized with the provided values.</returns>
    public static Treatment Create(string name, decimal price, TimeSpan duration, CertificationTypes certificationTypeRequired, int maxParticipants)
    {
        return new Treatment(name, price, duration, certificationTypeRequired, maxParticipants);
    }

    /// <summary>
    /// Sets the maximum number of participants for the entity.
    /// </summary>
    /// <param name="newMaxParticipantsAmount">The new maximum number of participants. Must be at least 1.</param>
    /// <exception cref="DomainException">Thrown when newMaxParticipantsAmount is less than 1.</exception>
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
