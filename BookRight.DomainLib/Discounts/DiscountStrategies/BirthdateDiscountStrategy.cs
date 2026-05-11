using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class BirthdateDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountProcentage;
    private readonly int _maxUsePerYear;

    public BirthdateDiscountStrategy()
    {
        _discountProcentage = 25;
        _maxUsePerYear = 1;
        
    }

    public DiscountTypes discountTypes => DiscountTypes.BirthdayMonth;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = 0;

        if (input.BookingDate.Month == input.CustomerBirthdate.Month &&
            input.NumberOfBirthdayDiscountUsed < _maxUsePerYear)
            price = input.NormalPrice * (1 - _discountProcentage / 100);

        return new PriceCalculatorResult(input.NormalPrice, price, discountTypes);
    }
}
