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

    public DiscountTypes DiscountTypes => DiscountTypes.BirthdayMonth;

    public PriceCalculatorResult CalculatePrice(PriceCalculatorInput input)
    {
        decimal price = input.NormalPrice;
        bool IsApplicable = false;

        if (input.BookingDate.Month == input.CustomerBirthdate.Month &&
            input.NumberOfBirthdayDiscountUsed < _maxUsePerYear)
        {
            price = input.NormalPrice * (1 - _discountProcentage / 100);
            IsApplicable = true;
        }


        return new PriceCalculatorResult(input.NormalPrice, price, DiscountTypes, IsApplicable);
    }
}
