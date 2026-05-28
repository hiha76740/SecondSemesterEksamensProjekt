using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

/// <summary>
/// Represents a discount strategy that applies no discount and leaves the price unchanged.
/// </summary>
/// <remarks>Always reports DiscountTypes.None and returns the input NormalPrice as both the original and final
/// price; applicability is always true.</remarks>
public class NoDiscountStrategy : IDiscountStrategy
{
    public DiscountTypes DiscountTypes => DiscountTypes.None;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        bool IsApplicable = true;
        return new PriceCalculatorResult(input.NormalPrice, input.NormalPrice, DiscountTypes, IsApplicable);
    }
}
