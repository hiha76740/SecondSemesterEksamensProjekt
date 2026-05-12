using BookRight.DomainLib.Enums;
using System.Collections.Immutable;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class CampaignDiscountStrategy : IDiscountStrategy
{
    public DiscountTypes discountTypes => DiscountTypes.Campaign;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = 0;
        DateOnly bookingDate = DateOnly.FromDateTime(input.BookingDate);

        var bestCampaign = input.ActiveCampaigns
            .Where(c => 
            bookingDate >= c.CampaignPeriod.From && 
            bookingDate <= c.CampaignPeriod.To &&
            c.AssignedTreatments.Any(
                at => at.Equals(
                    input.Treatments.Select(t => t.Id))))
            .OrderByDescending(c => c.DiscountProcentage)
            .FirstOrDefault();

        if (bestCampaign != null)
            price = input.NormalPrice * (1 - bestCampaign.DiscountProcentage / 100);

        return new PriceCalculatorResult(input.NormalPrice, price, discountTypes);
    }
}
