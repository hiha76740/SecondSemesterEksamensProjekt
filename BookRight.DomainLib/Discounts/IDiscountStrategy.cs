using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts;

public interface IDiscountStrategy
{
    DiscountTypes discountTypes { get; }
    PriceCalculatorResult CalculatePrice(PriceCalculatorInput input);
}
