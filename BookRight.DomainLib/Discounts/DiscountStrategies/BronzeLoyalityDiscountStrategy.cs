using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class BronzeLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountProcentage;

    public BronzeLoyalityDiscountStrategy()
    {
        _discountProcentage = 5;
    }

    public DiscountTypes discountTypes => DiscountTypes.LoyaltyBronze;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = 0;

        if (input.CustomerBookingsLast12Months >= 3000 && input.CustomerBookingsLast12Months <= 10000)
            price = input.NormalPrice * (1 - _discountProcentage / 100);

        return new PriceCalculatorResult(input.NormalPrice, price, discountTypes);
    }
}
