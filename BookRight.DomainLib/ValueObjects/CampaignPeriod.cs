using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record CampaignPeriod
{
    public DateOnly From { get; init; }
    public DateOnly To { get; init; }

    public CampaignPeriod(DateOnly from, DateOnly to)
    {
        if (from > to)
            throw new DomainException("End can not be before start");

        To = to;
        From = from;
    }
}