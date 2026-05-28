using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

/// <summary>
/// Applies a Silver loyalty discount strategy that computes a discounted price for customers with bookingsum of 10,001 to 25,000
/// in the last 12 months.
/// </summary>
/// <remarks>Uses a fixed 10% discount configured in the constructor and identifies the discount as
/// DiscountTypes.LoyaltySilver. Returns a PriceCalculatorResult containing the original price, the calculated price,
/// the discount type, and a flag indicating whether the discount was applied.</remarks>
public class SilverLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountPercentage;

    public SilverLoyalityDiscountStrategy()
    {
        _discountPercentage = 10;
    }

    public DiscountTypes DiscountTypes => DiscountTypes.LoyaltySilver;


    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.CustomerBookingsLast12Months >= 10001 && input.CustomerBookingsLast12Months <= 25000)
        {
            price = input.NormalPrice * (1 - _discountPercentage / 100);
            IsApplicable = true;
        }
            

        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
