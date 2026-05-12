using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class BronzeLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountProcentage;

    public BronzeLoyalityDiscountStrategy()
    {
        _discountProcentage = 5;
    }

    public DiscountTypes DiscountTypes => DiscountTypes.LoyaltyBronze;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.CustomerBookingsLast12Months >= 3000 && input.CustomerBookingsLast12Months <= 10000)
        {
            price = input.NormalPrice * (1 - _discountProcentage / 100);
            IsApplicable = true;
        }

        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
