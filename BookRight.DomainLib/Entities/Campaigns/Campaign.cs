using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Campaigns;

/// <summary>
/// Represents an aggregate root for a marketing campaign that applies a percentage discount to assigned treatments
/// within a defined period.
/// </summary>
/// <remarks>Instantiate via the Create factory. Invariants: Name must be non-empty; DiscountPercentage must be
/// greater than zero; at least one treatment must be assigned; CampaignPeriod.From must be after today's date. Status
/// is set to Active on creation and can be set to Inactive. A parameterless constructor is provided for EF.</remarks>
public class Campaign : AggregateRoot
{
    public string Name { get; init; } = null!;
    public decimal DiscountPercentage { get; init; }
    public CampaignPeriod CampaignPeriod { get; init; } = null!;
    public IReadOnlyList<Guid> AssignedTreatments { get; init; } = null!;
    public CampaignStatus Status { get; private set; }

    /// <summary>
    /// Creates a Campaign with the specified name, discount percentage, campaign period, and assigned treatments.
    /// </summary>
    /// <param name="name">Campaign name.</param>
    /// <param name="discountPercentage">Discount percentage applied by the campaign.</param>
    /// <param name="campaignPeriod">Campaign active period.</param>
    /// <param name="assignedTreatments">Identifiers of treatments assigned to the campaign.</param>
    /// <returns>A new Campaign instance initialized with the specified values.</returns>
    public static Campaign Create(string name, decimal discountPercentage, CampaignPeriod campaignPeriod, IReadOnlyList<Guid> assignedTreatments)
    {
        return new Campaign(name, discountPercentage, campaignPeriod, assignedTreatments);
    }

    /// <summary>
    /// Sets the campaign status to Inactive.
    /// </summary>
    /// <exception cref="DomainException">Thrown if the campaign is already set to Inactive.</exception>
    public void SetInaktive()
    {
        if (Status == CampaignStatus.Inactive)
            throw new DomainException("Campain already set to Inactive");

        Status = CampaignStatus.Inactive;
    }

    /// <summary>
    /// Initializes a new Campaign with the specified name, discount percentage, campaign period, and assigned
    /// treatments.
    /// </summary>
    /// <param name="name">Name of the campaign; cannot be null, empty, or whitespace.</param>
    /// <param name="discountPercentage">Discount percentage applied to the campaign; must be greater than zero.</param>
    /// <param name="campaignPeriod">Campaign period specifying start and end dates; start date must be after today's date.</param>
    /// <param name="assignedTreatments">Read-only list of treatment identifiers assigned to the campaign; must contain at least one item.</param>
    /// <exception cref="DomainException">Thrown when validation fails: name is null or whitespace; discountPercentage is less than or equal to zero;
    /// campaignPeriod starts on or before today; or assignedTreatments is empty.</exception>
    private Campaign(string name, decimal discountPercentage, CampaignPeriod campaignPeriod, IReadOnlyList<Guid> assignedTreatments)
    {
        if (assignedTreatments.Count < 1)
            throw new DomainException("Campaign must have atleast 1 treatment assigned");

        if (campaignPeriod.From <= DateOnly.FromDateTime(DateTime.Today))
            throw new DomainException("Start date must be after todays date");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Campain must have a name");

        if (discountPercentage <= 0)
            throw new DomainException("Discount procentage can not be 0 or less");

        Id = Guid.NewGuid();
        Name = name;
        DiscountPercentage = discountPercentage;
        CampaignPeriod = campaignPeriod;
        AssignedTreatments = assignedTreatments;
        Status = CampaignStatus.Active;
    }

    private Campaign() { }
}
