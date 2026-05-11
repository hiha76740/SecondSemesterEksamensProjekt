using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class SilverLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountProcentage;

    public SilverLoyalityDiscountStrategy()
    {
        _discountProcentage = 10;
    }

    public DiscountTypes discountTypes => DiscountTypes.LoyaltySilver;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = 0;

        if (input.CustomerBookingsLast12Months >= 10001 && input.CustomerBookingsLast12Months <= 25000)
            price = input.NormalPrice * (1 - _discountProcentage / 100);

        return new PriceCalculatorResult(input.NormalPrice, price, discountTypes);
    }
}
