using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Campaigns;

public class Campaign : AggregateRoot
{
    public string Name { get; init; }
    public decimal DiscountProcentage { get; init; }
    public CampaignPeriod CampaignPeriod { get; init; }
    public Guid TreatmentId { get; init; }
    public CampaignStatus Status { get; private set; }


    public static Campaign Create(string name, decimal discountProcentage, CampaignPeriod campaignPeriod, Guid treatmentId)
    {
        return new Campaign(name, discountProcentage, campaignPeriod, treatmentId);
    }

    private Campaign(string name, decimal discountProcentage, CampaignPeriod campaignPeriod, Guid treatmentId)
    {
        if (campaignPeriod.From <= DateTime.Today)
            throw new DomainException("Start date must be after todays date");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Campain must have a name");

        if (discountProcentage <= 0)
            throw new DomainException("Discount procentage can not be 0 or less");


        Name = name;
        DiscountProcentage = discountProcentage;
        CampaignPeriod = campaignPeriod;
        TreatmentId = treatmentId;
        Status = CampaignStatus.Active;
    }
}
