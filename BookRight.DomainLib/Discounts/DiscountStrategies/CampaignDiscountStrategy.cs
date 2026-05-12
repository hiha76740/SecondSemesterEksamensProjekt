using BookRight.DomainLib.Enums;
using System.Collections.Immutable;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class CampaignDiscountStrategy : IDiscountStrategy
{
    public DiscountTypes DiscountTypes => DiscountTypes.Campaign;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;
        DateOnly bookingDate = DateOnly.FromDateTime(input.BookingDate);

        var treatmentIds = input.Treatments.Select(t => t.Id).ToList();

        var bestCampaign = input.ActiveCampaigns
            .Where(c =>
            bookingDate >= c.CampaignPeriod.From &&
            bookingDate <= c.CampaignPeriod.To &&
            c.AssignedTreatments.Any(
                at => treatmentIds.Contains(at)))
            .OrderByDescending(c => c.DiscountProcentage)
            .FirstOrDefault();

        if (bestCampaign != null)
        {
            price = input.NormalPrice * (1 - bestCampaign.DiscountProcentage / 100);
            IsApplicable = true;
        }


        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
