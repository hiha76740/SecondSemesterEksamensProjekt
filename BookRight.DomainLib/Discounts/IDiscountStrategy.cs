using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts;

public interface IDiscountStrategy
{
    DiscountTypes DiscountTypes { get; }
    PriceCalculatorResult CalculatePrice(PriceCalculatorInput input);
}
