namespace BookRight.DomainLib.Entities.Treatments;

public class Treatment : AggregateRoot
{
    public string? Name { get; protected set; }

    public decimal Price { get; protected set; }

    public TimeSpan Duration { get; protected set; }

    // TODO: Tilføj Certification Type Value Object property

    protected Treatment()
    {
        Id = Guid.NewGuid();
    }
}
