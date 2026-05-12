using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class NoDiscountStrategy : IDiscountStrategy
{
    public DiscountTypes DiscountTypes => DiscountTypes.None;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        bool IsApplicable = true;
        return new PriceCalculatorResult(input.NormalPrice, input.NormalPrice, DiscountTypes, IsApplicable);
    }
}
