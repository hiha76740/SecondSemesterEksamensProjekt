using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts;

/// <summary>
/// Represents a strategy that calculates discounts for pricing inputs and exposes which DiscountTypes it supports.
/// </summary>
/// <remarks>Implementations should be deterministic and thread-safe where possible. Keep each implementation
/// focused on a single discount behavior, validate inputs, and return a PriceCalculatorResult containing adjusted
/// totals and any applied discount metadata. Avoid external side effects.</remarks>
public interface IDiscountStrategy
{
    DiscountTypes DiscountTypes { get; }
    PriceCalculatorResult CalculatePrice(PriceCalculatorInput input);
}
