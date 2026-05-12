using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class GoldLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountProcentage;

    public GoldLoyalityDiscountStrategy()
    {
        _discountProcentage = 15;
    }

    public DiscountTypes DiscountTypes => DiscountTypes.LoyaltyGold;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.CustomerBookingsLast12Months > 25000)
        {
            price = input.NormalPrice * (1 - _discountProcentage / 100);
            IsApplicable = true;
        }


        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
