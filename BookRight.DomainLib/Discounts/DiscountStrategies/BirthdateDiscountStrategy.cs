using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Discounts.DiscountStrategies;

public class BirthDateDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountPercentage;
    private readonly int _maxUsePerYear;

    public BirthDateDiscountStrategy()
    {
        _discountPercentage = 25;
        _maxUsePerYear = 1;

    }

    public DiscountTypes DiscountTypes => DiscountTypes.BirthdayMonth;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.BookingDate.Month == input.CustomerBirthDate.Month &&
            input.NumberOfBirthdayDiscountUsed < _maxUsePerYear)
        {
            price = input.NormalPrice * (1 - _discountPercentage / 100);
            IsApplicable = true;
        }


        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
