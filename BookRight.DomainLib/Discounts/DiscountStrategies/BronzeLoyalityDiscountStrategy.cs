using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

/// <summary>
/// Calculates a fixed 5% loyalty discount for customers with a bookingsum of 3,000 to 10,000 in the last 12 months (Bronze
/// level).
/// </summary>
/// <remarks>Applies a 5% reduction to NormalPrice when CustomerBookingsLast12Months is between 3,000 and 10,000
/// inclusive. Produces a PriceCalculatorResult containing the original price, the discounted price,
/// DiscountTypes.LoyaltyBronze, and an applicability flag.</remarks>
public class BronzeLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountPercentage;

    public BronzeLoyalityDiscountStrategy()
    {
        _discountPercentage = 5;
    }

    public DiscountTypes DiscountTypes => DiscountTypes.LoyaltyBronze;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.CustomerBookingsLast12Months >= 3000 && input.CustomerBookingsLast12Months <= 10000)
        {
            price = input.NormalPrice * (1 - _discountPercentage / 100);
            IsApplicable = true;
        }

        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
