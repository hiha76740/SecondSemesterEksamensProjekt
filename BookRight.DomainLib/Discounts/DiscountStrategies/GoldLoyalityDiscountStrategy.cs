using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

/// <summary>
/// Loyalty discount strategy for gold customers that applies a fixed 15% discount when a customer’s bookingsum in the
/// last 12 months exceed 25,000.
/// </summary>
/// <remarks>Implements IDiscountStrategy. CalculatePrice returns a PriceCalculatorResult containing the original
/// price, the discounted price, DiscountTypes.LoyaltyGold, and an IsApplicable flag. The discount percentage is
/// configured in the constructor and the instance is immutable and safe to reuse.</remarks>
public class GoldLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountPercentage;

    public GoldLoyalityDiscountStrategy()
    {
        _discountPercentage = 15;
    }

    public DiscountTypes DiscountTypes => DiscountTypes.LoyaltyGold;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.CustomerBookingsLast12Months > 25000)
        {
            price = input.NormalPrice * (1 - _discountPercentage / 100);
            IsApplicable = true;
        }


        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
