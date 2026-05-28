using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

/// <summary>
/// Calculates a discounted price using the best matching active campaign for the booking date and selected treatments.
/// </summary>
/// <remarks>Selects campaigns whose CampaignPeriod contains the booking date and that target any of the provided
/// treatments, chooses the campaign with the highest DiscountPercentage, computes the discounted price as NormalPrice *
/// (1 - DiscountPercentage / 100), and returns a PriceCalculatorResult containing the original price, the computed
/// price, the Campaign discount type, and whether a campaign was applied.</remarks>
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
            .OrderByDescending(c => c.DiscountPercentage)
            .FirstOrDefault();

        if (bestCampaign != null)
        {
            price = input.NormalPrice * (1 - bestCampaign.DiscountPercentage / 100);
            IsApplicable = true;
        }


        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
