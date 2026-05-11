using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts;

public record PriceCalculatorResult(decimal NormalPrice, decimal FinalPrice, DiscountTypes DiscountType);