using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments;

public class Treatment : AggregateRoot
{
    public string? Name { get; protected set; }

    public decimal Price { get; protected set; }

    public TimeSpan Duration { get; protected set; }

    public CertificationTypes CertificationRequired { get; protected set; }

    protected Treatment()
    {
        Id = Guid.NewGuid();
    }
}
