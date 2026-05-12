using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record CampaignPeriod
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }

    public CampaignPeriod(DateTime from, DateTime to)
    {
        if (from >= to)
            throw new DomainException("End can not be before or Equal to start");

        To = to;
        From = from;
    }
}