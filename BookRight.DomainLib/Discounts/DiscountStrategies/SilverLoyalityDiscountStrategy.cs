using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class SilverLoyalityDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountProcentage;

    public SilverLoyalityDiscountStrategy()
    {
        _discountProcentage = 10;
    }

    public DiscountTypes DiscountTypes => DiscountTypes.LoyaltySilver;


    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.CustomerBookingsLast12Months >= 10001 && input.CustomerBookingsLast12Months <= 25000)
        {
            price = input.NormalPrice * (1 - _discountProcentage / 100);
            IsApplicable = true;
        }
            

        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
